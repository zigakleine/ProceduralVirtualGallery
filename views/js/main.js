import * as THREE from '../lib/three.module.js';
class Main {

    constructor() {

        this.scene =  new THREE.Scene();
        this.camera = new THREE.PerspectiveCamera(75, window.innerWidth/window.innerHeight, 0.1, 1000);
        this.renderer = new THREE.WebGLRenderer({antialias: true});

        this.windowResizeListener = window.addEventListener("resize", this.onWindowResize);

    }

    initWindow() {

        this.renderer.setClearColor("#e5e5e5");
        this.renderer.setSize(window.innerWidth, window.innerHeight);
        document.body.appendChild(this.renderer.domElement);



        this.camera.position.set(0,0,5);

    }

    generateScene() {

        this.light = new THREE.PointLight(0xFFFFFF, 1, 500);

        this.light.position.set(10,0,25);
        this.scene.add(this.light);

        this.material = new THREE.MeshLambertMaterial({color: 0xFFCC00});
        this.geometry = new THREE.BoxGeometry(1, 1, 1);
        this.mesh = new THREE.Mesh(this.geometry, this.material);

        this.mesh.position.set(0, 0, 0);
        this.scene.add(this.mesh);

    }

    static onWindowResize() {
        console.log("cam" + this.camera);
        this.renderer.setSize(window.innerWidth, window.innerHeight);
        this.camera.aspect = window.innerWidth/window.innerHeight;
        this.camera.updateProjectionMatrix();
    }

    render() {

        requestAnimationFrame(() => {this.render();});
        this.mesh.rotation.x += 0.01;
        this.mesh.rotation.y += 0.01;
        this.renderer.render(this.scene, this.camera);

    }


}

let main = new Main();
main.initWindow();
main.generateScene();
main.render();



