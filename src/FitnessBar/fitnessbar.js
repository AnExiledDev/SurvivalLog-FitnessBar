// Fitness Bar: a thin Fitness strip under the Life bar on the HUD (the CoreUI1 page), with a hover box that says what
// Fitness is, what it is now and what that means. The plugin hands it the value every couple of seconds through apply.
// Reads and draws only; nothing the game keeps is changed.
(function () {
    if (window.__slfitness) return;

    // The game's own line (AttributeStandard_TIPSDes_4): below 30 the player is Weakened.
    var WEAKENED_BELOW = 30;
    // A warning before that line.
    var LOW_BELOW = 40;

    var STRIP = 'slfb';
    var HOST = 'slfb-host';
    var STYLE_ID = 'slfb-style';
    var LIFE_ICON = '\u2764';
    // CoreUI1 draws the Life bar as the fourth row and gives it its shield icon by that index too.
    var LIFE_INDEX = 3;

    var TEXTS = {
        en: {
            name: 'Fitness',
            about: 'High Fitness lowers your chance of getting sick. Below 30 you become Weakened. Staying Hungry or Fatigued for too long lowers it.',
            weakened: 'Below 30, so you are Weakened.',
            low: function (left) { return 'Close to Weakened: ' + left + ' more lost and it drops below 30.'; },
            fine: function (above) { return above + ' above 30, where Weakened starts.'; },
            raise: 'Food that gives Fitness raises it.'
        },
        zh: {
            name: '健康',
            about: '良好的健康可以降低生病的概率。低于30会陷入「亚健康」。长期「饥饿」或「疲劳」会降低健康。',
            weakened: '低于30，你处于「亚健康」状态。',
            low: function (left) { return '接近「亚健康」：再降低 ' + left + ' 点就会低于30。'; },
            fine: function (above) { return '比「亚健康」的界线30高 ' + above + ' 点。'; },
            raise: '食用带有健康加成的食物可以提高健康。'
        }
    };

    // The look of the game's own bars (.bar-mask, .progress-text, .status-tooltip), at a strip's height. The Life row
    // gets room below it, so the strip covers nothing; the hover box opens upward from the strip's bottom edge, as the
    // game's last-row boxes do, so it stays on screen.
    var CSS = [
        '.' + HOST + '{position:relative;margin-bottom:16px}',
        '.' + STRIP + '{position:absolute;right:0;top:calc(100% + 3px);width:168px;height:12px}',
        '.' + STRIP + ' > .slfb-mask{position:relative;width:100%;height:100%;box-sizing:border-box;overflow:hidden;background-color:rgba(0,0,0,.5);border:1px solid rgba(255,255,255,.12);border-radius:2.8px;box-shadow:1.4px 1.4px 3.5px rgba(0,0,0,.5)}',
        '.slfb-fill{height:100%;width:100%;transform-origin:left;transition:transform .3s ease,background-color .3s ease;background-color:rgba(76,175,80,.55)}',
        '.slfb-low .slfb-fill{background-color:rgba(255,193,7,.55)}',
        '.slfb-weak .slfb-fill{background-color:rgba(211,47,47,.7)}',
        '.slfb-text{position:absolute;inset:0;display:flex;align-items:center;justify-content:space-between;padding:0 5px;color:#fff;font-weight:bold;font-size:9px;line-height:10px;text-shadow:.7px .7px 1.4px #000,0 0 3.5px rgba(0,0,0,.8)}',
        '.slfb-weak .slfb-text{color:#FFEBEE;text-shadow:0 0 2px rgba(255,0,0,.8)}',
        '.' + STRIP + ' > .slfb-tip{top:auto;bottom:0;transform:none}',
        '.' + STRIP + ' > .slfb-tip::after{top:auto;bottom:1px;transform:none}',
        '.' + STRIP + ':hover > .slfb-tip{opacity:1;visibility:visible}',
        '.slfb-state{margin-top:6px;padding-top:5px;border-top:1px solid rgba(255,255,255,.15)}',
        '.slfb-weak .slfb-state{color:#ff8a80}',
        '.slfb-low .slfb-state{color:#FFC107}'
    ].join('\n');

    function apply(reading) {
        var row = lifeRow();

        addStyle();
        dropStrays(row);

        if (!row) return 'nobar';

        var strip = stripIn(row);

        if (!(reading && reading.max > 0 && reading.value >= 0)) {
            if (strip) remove(row, strip);

            return 'noplayer';
        }

        if (!strip) strip = add(row);

        draw(strip, reading);

        return 'ok';
    }

    // The row whose bar the game marks with the heart; the fourth row if a game update drops the mark.
    function lifeRow() {
        var rows = document.querySelectorAll('.status-panel > .status-row');
        var list = attrList();

        if (list && list.length === rows.length) {
            for (var i = 0; i < list.length; i++) {
                if (list[i] && String(list[i].attrIcon || '').indexOf(LIFE_ICON) >= 0) return rows[i];
            }
        }

        return rows.length > LIFE_INDEX ? rows[LIFE_INDEX] : null;
    }

    function attrList() {
        var app = document.getElementById('app');
        var ss = app && app._vnode && app._vnode.component ? app._vnode.component.setupState : null;

        return ss && ss.state && Array.isArray(ss.state.attrList) ? ss.state.attrList : null;
    }

    // A strip left on a row that is no longer the Life row (the rows were drawn again in another order) goes.
    function dropStrays(row) {
        var strips = document.querySelectorAll('.' + STRIP);

        for (var i = 0; i < strips.length; i++) {
            if (strips[i].parentElement !== row) remove(strips[i].parentElement, strips[i]);
        }
    }

    function stripIn(row) {
        for (var i = 0; i < row.children.length; i++) {
            if (row.children[i].classList.contains(STRIP)) return row.children[i];
        }

        return null;
    }

    function add(row) {
        var strip = element('div', STRIP);
        var mask = strip.appendChild(element('div', 'slfb-mask'));
        var text = element('div', 'slfb-text');
        var tip = strip.appendChild(element('div', 'status-tooltip slfb-tip'));
        var header = tip.appendChild(element('div', 'st-header'));

        // The page hands the game only the areas marked this way, so the pointer over the strip is not taken as a
        // click on the world behind it.
        strip.setAttribute('data-interactive', '');
        mask.appendChild(element('div', 'slfb-fill'));
        mask.appendChild(text);
        text.appendChild(element('span', 'slfb-name'));
        text.appendChild(element('span', 'slfb-value'));
        header.appendChild(element('div', 'st-title'));
        header.appendChild(element('div', 'st-value'));
        tip.appendChild(element('div', 'st-body slfb-about'));
        tip.appendChild(element('div', 'st-body slfb-state'));
        tip.appendChild(element('div', 'st-body slfb-raise'));

        row.classList.add(HOST);
        row.appendChild(strip);
        syncClicks();

        return strip;
    }

    function remove(row, strip) {
        row.removeChild(strip);
        row.classList.remove(HOST);
        syncClicks();
    }

    function draw(strip, reading) {
        var say = reading.zh ? TEXTS.zh : TEXTS.en;
        var value = Math.round(reading.value);
        var max = Math.round(reading.max);
        var level = levelOf(value);
        var fill = Math.max(0, Math.min(1, value / max));

        setLevel(strip, level);
        setText(strip, '.slfb-name', '\u271A ' + say.name);
        setText(strip, '.slfb-value', String(value));
        setText(strip, '.st-title', say.name);
        setText(strip, '.st-value', value + '/' + max);
        setText(strip, '.slfb-about', say.about);
        setText(strip, '.slfb-state', stateLine(say, level, value));
        setText(strip, '.slfb-raise', say.raise);

        var transform = 'scaleX(' + fill + ')';
        var bar = strip.querySelector('.slfb-fill');

        if (bar.style.transform !== transform) bar.style.transform = transform;
    }

    function levelOf(value) {
        if (value < WEAKENED_BELOW) return 'weak';
        if (value < LOW_BELOW) return 'low';

        return 'ok';
    }

    function stateLine(say, level, value) {
        if (level === 'weak') return say.weakened;
        if (level === 'low') return say.low(value - WEAKENED_BELOW + 1);

        return say.fine(value - WEAKENED_BELOW);
    }

    function setLevel(strip, level) {
        var levels = ['ok', 'low', 'weak'];

        for (var i = 0; i < levels.length; i++) {
            if (levels[i] === level) strip.classList.add('slfb-' + levels[i]);
            else strip.classList.remove('slfb-' + levels[i]);
        }
    }

    function setText(strip, selector, text) {
        var target = strip.querySelector(selector);

        if (target.textContent !== text) target.textContent = text;
    }

    function element(tagName, className) {
        var made = document.createElement(tagName);

        made.className = className;

        return made;
    }

    function addStyle() {
        if (document.getElementById(STYLE_ID)) return;

        var style = document.createElement('style');

        style.id = STYLE_ID;
        style.textContent = CSS;
        document.head.appendChild(style);
    }

    // The page's click-through measures the marked areas again after a change of layout; without it the strip's area
    // is learned only at the page's next own redraw.
    function syncClicks() {
        var core = pageGlobal('core');

        if (core && core.clickThrough && typeof core.clickThrough.syncNowAfterReflow === 'function') core.clickThrough.syncNowAfterReflow();
    }

    function pageGlobal(name) {
        try {
            return (0, eval)(name);
        } catch (e) {
            return undefined;
        }
    }

    window.__slfitness = { apply: apply, levelOf: levelOf, texts: TEXTS };
})();
