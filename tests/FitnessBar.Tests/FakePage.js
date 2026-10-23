// Just enough of the CoreUI1 page for fitnessbar.js to run in Jint: elements with classes, attributes, text and a
// parent, the few selectors the script uses, the Vue app's attrList, the page's click-through, and the HUD's stat rows
// drawn the way the game's template draws them. Test code only; nothing here ships.
var window = this;

var FakeElement = class {
    constructor(tagName, className) {
        var element = this;

        this.tagName = tagName;
        this.id = '';
        this.parentElement = null;
        this.children = [];
        this.textContent = '';
        this.attrs = {};
        this.classes = className ? className.split(' ') : [];
        this.style = {};
        this.classList = {
            contains: function (name) { return element.classes.indexOf(name) >= 0; },
            add: function (name) { if (element.classes.indexOf(name) < 0) element.classes.push(name); },
            remove: function (name) { element.classes = element.classes.filter(function (c) { return c !== name; }); }
        };
    }

    get className() { return this.classes.join(' '); }

    set className(value) { this.classes = String(value).split(' ').filter(function (c) { return c; }); }

    hasAttribute(name) { return Object.prototype.hasOwnProperty.call(this.attrs, name); }

    setAttribute(name, value) { this.attrs[name] = String(value); }

    appendChild(child) {
        if (child.parentElement) child.parentElement.removeChild(child);

        child.parentElement = this;
        this.children.push(child);

        return child;
    }

    removeChild(child) {
        var at = this.children.indexOf(child);

        if (at < 0) throw new Error('removeChild: not a child of this element');

        this.children.splice(at, 1);
        child.parentElement = null;

        return child;
    }

    // ".a > .b" is the child combinator: the last step names this element, each step before it the parent above.
    matches(selector) {
        var steps = selector.split('>').map(function (step) { return step.trim(); });
        var element = this;

        for (var i = steps.length - 1; i >= 0; i--) {
            if (!element || !matchesStep(element, steps[i])) return false;

            element = element.parentElement;
        }

        return true;
    }

    querySelectorAll(selector) {
        return descendantsOf(this).filter(function (element) { return element.matches(selector); });
    }

    querySelector(selector) {
        return this.querySelectorAll(selector)[0] || null;
    }
};

function matchesStep(element, step) {
    if (/^\.[\w-]+$/.test(step)) return element.classList.contains(step.slice(1));

    throw new Error('the fake page does not know the selector "' + step + '"');
}

function descendantsOf(root) {
    var found = [];

    root.children.forEach(function walk(child) {
        found.push(child);
        child.children.forEach(walk);
    });

    return found;
}

var document = {
    documentElement: new FakeElement('html'),
    getElementById: function (id) {
        return descendantsOf(document.documentElement).filter(function (element) { return element.id === id; })[0] || null;
    },
    createElement: function (tagName) { return new FakeElement(tagName); },
    querySelector: function (selector) { return document.documentElement.querySelector(selector); },
    querySelectorAll: function (selector) { return document.documentElement.querySelectorAll(selector); }
};

document.head = document.documentElement.appendChild(new FakeElement('head'));
document.body = document.documentElement.appendChild(new FakeElement('body'));

var core = { clickThrough: { syncs: 0, syncNowAfterReflow: function () { core.clickThrough.syncs++; } } };

// Draws the HUD's stat rows as CoreUI1's template does: the panel, then one row per attrList item with its buffs on
// the left and the bar on the right. icons is attrList's attrIcon per row; the game's four are the default.
// spec: { icons, hasNoVue }
function buildPage(spec) {
    var icons = spec.icons || ['\uD83C\uDF56', '\uD83E\uDDE0', '\u26A1', '\u2764'];
    var app = document.body.appendChild(new FakeElement('div'));
    var container = app.appendChild(new FakeElement('div', 'bottom-right-container'));
    var panel = container.appendChild(new FakeElement('div', 'status-panel'));
    var attrList = icons.map(function (icon) { return { attrIcon: icon }; });

    app.id = 'app';

    if (!spec.hasNoVue) app._vnode = { component: { setupState: { state: { attrList: attrList } } } };

    var rows = icons.map(function () {
        var row = panel.appendChild(new FakeElement('div', 'status-row'));
        var bar = new FakeElement('div', 'progress-bar-wrapper');

        row.appendChild(new FakeElement('div', 'buff-container'));
        row.appendChild(bar).setAttribute('data-interactive', '');
        bar.appendChild(new FakeElement('div', 'bar-mask'));
        bar.appendChild(new FakeElement('div', 'status-tooltip'));

        return row;
    });

    return { panel: panel, rows: rows, attrList: attrList };
}

// The strip on a row, or null.
function stripOn(row) {
    return row.children.filter(function (child) { return child.classList.contains('slfb'); })[0] || null;
}
