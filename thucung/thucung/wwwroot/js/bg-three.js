(function () {
    // Tự động tạo Canvas nền nếu chưa có
    var bgCanvas = document.getElementById("bg-three-canvas");
    if (!bgCanvas) {
        bgCanvas = document.createElement("canvas");
        bgCanvas.id = "bg-three-canvas";
        bgCanvas.style.position = "fixed";
        bgCanvas.style.top = "0";
        bgCanvas.style.left = "0";
        bgCanvas.style.width = "100vw";
        bgCanvas.style.height = "100vh";
        bgCanvas.style.pointerEvents = "none";
        bgCanvas.style.zIndex = "-1";
        document.body.appendChild(bgCanvas);
    }

    if (typeof THREE === "undefined") return;

    var scene = new THREE.Scene();
    var camera = new THREE.PerspectiveCamera(75, window.innerWidth / window.innerHeight, 1, 1000);
    camera.position.z = 400;

    var renderer = new THREE.WebGLRenderer({ canvas: bgCanvas, alpha: true, antialias: true });
    renderer.setSize(window.innerWidth, window.innerHeight);
    renderer.setPixelRatio(Math.min(window.devicePixelRatio, 2));

    // Tạo mạng lưới sóng hạt 3D (Interactive Particle Grid)
    var countX = 40, countY = 40;
    var numParticles = countX * countY;
    var positions = new Float32Array(numParticles * 3);
    var scales = new Float32Array(numParticles);

    var i = 0;
    for (var ix = 0; ix < countX; ix++) {
        for (var iy = 0; iy < countY; iy++) {
            positions[i * 3] = ix * 35 - (countX * 35) / 2;
            positions[i * 3 + 1] = 0;
            positions[i * 3 + 2] = iy * 35 - (countY * 35) / 2;
            scales[i] = 4;
            i++;
        }
    }

    var geometry = new THREE.BufferGeometry();
    geometry.setAttribute('position', new THREE.BufferAttribute(positions, 3));

    var material = new THREE.PointsMaterial({
        color: 0x00e5ff,
        size: 3,
        transparent: true,
        opacity: 0.6,
        blending: THREE.AdditiveBlending
    });

    var particles = new THREE.Points(geometry, material);
    scene.add(particles);

    // Xử lý di chuột tương tác 3D
    var mouseX = 0, mouseY = 0;
    document.addEventListener('mousemove', function (e) {
        mouseX = (e.clientX - window.innerWidth / 2) * 0.1;
        mouseY = (e.clientY - window.innerHeight / 2) * 0.1;
    });

    window.addEventListener('resize', function () {
        camera.aspect = window.innerWidth / window.innerHeight;
        camera.updateProjectionMatrix();
        renderer.setSize(window.innerWidth, window.innerHeight);
    });

    var count = 0;
    function animate() {
        requestAnimationFrame(animate);

        camera.position.x += (mouseX - camera.position.x) * 0.05;
        camera.position.y += (-mouseY + 200 - camera.position.y) * 0.05;
        camera.lookAt(scene.position);

        var posAttr = geometry.attributes.position;
        var idx = 0;
        for (var ix = 0; ix < countX; ix++) {
            for (var iy = 0; iy < countY; iy++) {
                posAttr.setY(idx, (Math.sin((ix + count) * 0.3) * 20) + (Math.sin((iy + count) * 0.5) * 20));
                idx++;
            }
        }
        posAttr.needsUpdate = true;
        count += 0.05;

        renderer.render(scene, camera);
    }

    animate();
})();