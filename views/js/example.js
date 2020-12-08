
Physijs.scripts.worker = './physijs/physijs_worker.js';
Physijs.scripts.ammo = '../lib/ammo.js';


let initScene = function() {
    let renderer = new THREE.WebGLRenderer({ antialias: true });
    renderer.setSize( window.innerWidth, window.innerHeight );

    document.body.appendChild( renderer.domElement );


    let scene = new Physijs.Scene();
    scene.setGravity(new THREE.Vector3( 0, -30, 0 ));
    scene.addEventListener(
        'update',
        function() {
            scene.simulate();
        }
    );

    let camera = new THREE.PerspectiveCamera(
        35,
        window.innerWidth / window.innerHeight,
        1,
        1000
    );
    camera.position.set( 60, 50, 60 );
    camera.lookAt( scene.position );
    scene.add( camera );

    // Light
    let light = new THREE.DirectionalLight( 0xFFFFFF );
    light.position.set( 20, 40, -15 );
    scene.add( light );

    // Loader

    // Materials
    let ground_material = Physijs.createMaterial(
        new THREE.MeshLambertMaterial({ color: 0xFFFFFF}),
        .8, // high friction
        .4 // low restitution
    );

    let box_material = Physijs.createMaterial(
        new THREE.MeshLambertMaterial(new THREE.MeshLambertMaterial({ color: 0xFFFFFF})),
        .4, // low friction
        .6 // high restitution
    );

    let ground = new Physijs.BoxMesh(
        new THREE.BoxGeometry(100, 1, 100),
        ground_material,
        0 // mass
    );


    ground.receiveShadow = true;
    scene.add( ground );

    for ( var i = 0; i < 10; i++ ) {
        let box = new Physijs.BoxMesh(
            new THREE.BoxGeometry( 4, 4, 4 ),
            box_material
        );
        box.position.set(
            Math.random() * 50 - 25,
            10 + Math.random() * 5,
            Math.random() * 50 - 25
        );
        box.rotation.set(
            Math.random() * Math.PI * 2,
            Math.random() * Math.PI * 2,
            Math.random() * Math.PI * 2
        );
        box.scale.set(
            Math.random() * 1 + .5,
            Math.random() * 1 + .5,
            Math.random() * 1 + .5
        );
        box.castShadow = true;
        scene.add( box );
        //boxes.push( box );
    }

    let render = function() {
        requestAnimationFrame( render );
        renderer.render( scene, camera );
    };

    requestAnimationFrame( render );
    scene.simulate();
};




window.onload = initScene;