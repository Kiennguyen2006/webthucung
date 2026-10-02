(function () {
    var world = document.getElementById("intro-world");
    var canvas = document.getElementById("intro-canvas");
    if (!world || !canvas || typeof THREE === "undefined") return;

    var homeUrl = world.getAttribute("data-home-url") || "/Kiemtrathongtin/Dangnhap";
    var skipButton = document.getElementById("intro-skip");
    var brandElem = document.getElementById("intro-brand");

    var finished = false;

    // 1. SCENE & CAMERA
    var scene = new THREE.Scene();
    var camera = new THREE.PerspectiveCamera(60, window.innerWidth / window.innerHeight, 1, 2000);
    camera.position.z = 400;

    var renderer = new THREE.WebGLRenderer({ canvas: canvas, antialias: true, alpha: true });
    renderer.setPixelRatio(Math.min(window.devicePixelRatio, 2));
    renderer.setSize(window.innerWidth, window.innerHeight);

    // 2. TẠO SPRITE HẠT SÁNG CỰC RỰC RỠ (GLOW SPRITE)
    function createLightSprite() {
        var c = document.createElement('canvas');
        c.width = 128;
        c.height = 128;
        var ctx = c.getContext('2d');

        var gradient = ctx.createRadialGradient(64, 64, 0, 64, 64, 64);
        gradient.addColorStop(0, 'rgba(255, 255, 255, 1)');
        gradient.addColorStop(0.2, 'rgba(0, 229, 255, 0.9)');
        gradient.addColorStop(0.5, 'rgba(224, 64, 251, 0.4)');
        gradient.addColorStop(1, 'rgba(0, 0, 0, 0)');

        ctx.fillStyle = gradient;
        ctx.fillRect(0, 0, 128, 128);

        return new THREE.CanvasTexture(c);
    }

    // 3. KHỞI TẠO QUẢ CẦU HẠT NĂNG LƯỢNG (MORPHING SPHERE)
    var particleCount = 4000;
    var geometry = new THREE.BufferGeometry();
    var positions = new Float32Array(particleCount * 3);
    var basePositions = new Float32Array(particleCount * 3);
    var colors = new Float32Array(particleCount * 3);

    var color1 = new THREE.Color(0x00e5ff); // Cyan
    var color2 = new THREE.Color(0xe040fb); // Purple
    var color3 = new THREE.Color(0x64ffda); // Mint

    var radius = 160;

    for (var i = 0; i < particleCount; i++) {
        // Phân bố hạt trên bề mặt khối cầu Fibonacci
        var phi = Math.acos(-1 + (2 * i) / particleCount);
        var theta = Math.sqrt(particleCount * Math.PI) * phi;

        var x = radius * Math.cos(theta) * Math.sin(phi);
        var y = radius * Math.sin(theta) * Math.sin(phi);
        var z = radius * Math.cos(phi);

        positions[i * 3] = x;
        positions[i * 3 + 1] = y;
        positions[i * 3 + 2] = z;

        basePositions[i * 3] = x;
        basePositions[i * 3 + 1] = y;
        basePositions[i * 3 + 2] = z;

        // Phối màu ma thuật theo tọa độ
        var mixedColor = color1.clone().lerp(color2, (y + radius) / (2 * radius));
        if (Math.random() > 0.7) mixedColor.lerp(color3, 0.8);

        colors[i * 3] = mixedColor.r;
        colors[i * 3 + 1] = mixedColor.g;
        colors[i * 3 + 2] = mixedColor.b;
    }

    geometry.setAttribute('position', new THREE.BufferAttribute(positions, 3));
    geometry.setAttribute('color', new THREE.BufferAttribute(colors, 3));

    var material = new THREE.PointsMaterial({
        size: 14,
        map: createLightSprite(),
        vertexColors: true,
        blending: THREE.AdditiveBlending,
        depthTest: false,
        transparent: true,
        opacity: 0.95
    });

    var spherePoints = new THREE.Points(geometry, material);
    scene.add(spherePoints);

    // 4. TƯƠNG TÁC CON TRỎ CHUỘT (INTERACTIVE MOUSE)
    var mouseX = 0, mouseY = 0;
    var targetRotationX = 0, targetRotationY = 0;

    document.addEventListener('mousemove', function (e) {
        mouseX = (e.clientX - window.innerWidth / 2) * 0.0015;
        mouseY = (e.clientY - window.innerHeight / 2) * 0.0015;
    });

    function onWindowResize() {
        camera.aspect = window.innerWidth / window.innerHeight;
        camera.updateProjectionMatrix();
        renderer.setSize(window.innerWidth, window.innerHeight);
    }
    window.addEventListener('resize', onWindowResize, false);

    setTimeout(function () {
        if (brandElem) brandElem.classList.add("is-visible");
    }, 500);
    var clock = new THREE.Clock();

    function animate() {
        if (finished) return;
        requestAnimationFrame(animate);

        var time = clock.getElapsedTime();
        targetRotationY += mouseX * 0.05;
        targetRotationX += mouseY * 0.05;

        spherePoints.rotation.y = time * 0.2 + targetRotationY;
        spherePoints.rotation.x = Math.sin(time * 0.1) * 0.2 + targetRotationX;

        // Thuật toán sóng co bóp biến dạng (Morphing Wave)
        var posAttr = geometry.attributes.position;
        for (var i = 0; i < particleCount; i++) {
            var bx = basePositions[i * 3];
            var by = basePositions[i * 3 + 1];
            var bz = basePositions[i * 3 + 2];

            // Tốc độ và biên độ sóng biến dạng
            var wave = Math.sin(time * 2.5 + bx * 0.02 + by * 0.02 + bz * 0.02) * 18;
            var pulse = Math.cos(time * 1.5 + i * 0.1) * 8;

            var factor = 1 + (wave + pulse) / radius;

            posAttr.setXYZ(i, bx * factor, by * factor, bz * factor);
        }
        posAttr.needsUpdate = true;

        renderer.render(scene, camera);
    }

    animate();

    function goHome() {
        if (finished) return;
        finished = true;

        world.classList.add("is-wash");
        setTimeout(function () {
            window.location.replace(homeUrl);
        }, 1200);
    }

    var autoTimer = setTimeout(goHome, 8000);

    if (skipButton) {
        skipButton.addEventListener("click", function () {
            clearTimeout(autoTimer);
            goHome();
        });
    }
})();