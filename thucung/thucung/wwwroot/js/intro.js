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

    function later(ms, fn) {
        timers.push(window.setTimeout(fn, ms));
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
        }

        world.classList.add("is-wash");

        window.setTimeout(function () {
            window.location.replace(homeUrl);
        }, 1400);
    }

    if (!forceReplay) {
        try {
            if (sessionStorage.getItem(storageKey) === "1") {
                window.location.replace(homeUrl);
                return;
            }
        } catch (e) {
        }
    }

    if (window.matchMedia("(prefers-reduced-motion: reduce)").matches) {
        world.classList.add(
            "is-scene",
            "is-alive",
            "is-look",
            "is-door",
            "is-running",
            "is-open",
            "is-enter",
            "is-wash"
        );

        later(1800, goHome);

        if (skipButton) {
            skipButton.addEventListener("click", goHome);
        }

        return;
    }

    /*
        0.0s
        Bắt đầu dựng thế giới.
    */
    later(100, function () {
        world.classList.add("is-scene");
    });

    /*
        3.2s
        Thú cưng xuất hiện.
    */
    later(3200, function () {
        world.classList.add("is-alive");
    });

    /*
        3.2s -> 11.5s
        Thú cưng chơi đùa khá lâu.
        Người xem có thời gian nhìn toàn cảnh.
    */

    /*
        11.5s
        Thú cưng bắt đầu chú ý về bên phải.
    */
    later(11500, function () {
        world.classList.add("is-look");
    });

    /*
        14.0s
        CỬA MỚI XUẤT HIỆN.
        Không xuất hiện quá sớm.
    */
    later(14000, function () {
        world.classList.add("is-door");
    });

    /*
        16.2s
        Sau khi người xem đã thấy cửa,
        thú cưng mới bắt đầu chạy.
    */
    later(16200, function () {
        world.classList.add("is-running");
    });

    /*
        18.8s
        Cửa mở chậm.
    */
    later(18800, function () {
        world.classList.add("is-open");
    });

    /*
        20.3s
        Thú cưng chạy qua cửa.
    */
    later(20300, function () {
        world.classList.add("is-enter");
    });

    /*
        21500ms
        Ánh sáng vàng bắt đầu phủ màn hình.
    */
    later(21500, function () {
        world.classList.add("is-wash");
    });

    /*
        22900ms
        Sang website chính.
    */
    later(22900, goHome);

    if (skipButton) {
        skipButton.addEventListener("click", goHome);
    }
})();