(function () {
    var world = document.getElementById("intro-world");
    if (!world) {
        return;
    }

    var homeUrl = world.getAttribute("data-home-url") || "/Home/Index";
    var skipButton = document.getElementById("intro-skip");
    var timers = [];
    var finished = false;
    var storageKey = "kmna-intro-seen";
    var params = new URLSearchParams(window.location.search);
    var forceReplay = params.get("replay") === "1";

    function clearTimers() {
        timers.forEach(function (id) {
            window.clearTimeout(id);
        });
        timers = [];
    }

    function goHome() {
        if (finished) {
            return;
        }
        finished = true;
        clearTimers();
        try {
            sessionStorage.setItem(storageKey, "1");
        } catch (e) {
            /* ignore private-mode storage errors */
        }
        world.classList.add("is-wash");
        window.setTimeout(function () {
            window.location.replace(homeUrl);
        }, 720);
    }

    function later(ms, fn) {
        timers.push(window.setTimeout(fn, ms));
    }

    if (!forceReplay) {
        try {
            if (sessionStorage.getItem(storageKey) === "1") {
                window.location.replace(homeUrl);
                return;
            }
        } catch (e) {
            /* continue playing intro */
        }
    }

    if (window.matchMedia("(prefers-reduced-motion: reduce)").matches) {
        world.classList.add("is-scene", "is-alive", "is-door", "is-open");
        later(900, goHome);
        if (skipButton) {
            skipButton.addEventListener("click", goHome);
        }
        return;
    }

    later(40, function () {
        world.classList.add("is-scene");
    });

    later(2000, function () {
        world.classList.add("is-alive");
    });

    later(6000, function () {
        world.classList.add("is-look");
    });

    later(6800, function () {
        world.classList.add("is-running");
    });

    later(8000, function () {
        world.classList.add("is-door");
    });

    later(9000, function () {
        world.classList.add("is-open");
    });

    later(9800, function () {
        world.classList.add("is-enter");
    });

    later(11800, function () {
        world.classList.add("is-wash");
    });

    later(13200, goHome);

    if (skipButton) {
        skipButton.addEventListener("click", goHome);
    }
})();
