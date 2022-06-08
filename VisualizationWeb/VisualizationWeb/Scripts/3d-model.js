const IS_ROTATING = false;
const AUTO_ROTATION_STEP = 0.005;
const ROTATION_STEP = 0.00025;
const IS_WATER_TRANSPARENT = true;
const WATER_TRANSPARENCY = 0.5;
const BACKGROUND_COLOR = '#fff';

var translateY = 1.05;

var cubeLengths = {
    width: 2,
    height: .5,
    depth: 2,
};

var cubeWaterLengths = {
    width: 4,
    height: 1,
    depth: 4,
};

var height = $('#CityRotationChartPanel').width(); // window.innerWidth / 2;
var width = $('#CityRotationChartPanel').width(); // window.innerWidth / 2;

var scene = new THREE.Scene();
scene.background = new THREE.Color(BACKGROUND_COLOR);

var ambientLight = new THREE.AmbientLight(0xffffff, 2);
scene.add(ambientLight);
var pointLight = new THREE.PointLight(0xffffff, 0.5);
scene.add(pointLight);

var camera = new THREE.PerspectiveCamera(75, width / height, 0.1, 1000);

//Creates renderer and adds it to the DOM
var renderer,
    cubeAxis,
    plane,
    myCanvas = document.getElementById('3d-model');

var controls = new THREE.OrbitControls(camera, myCanvas);

renderer = new THREE.WebGLRenderer({canvas: myCanvas, antialias: true});
renderer.setSize(width, height);

//BoxGeometry (makes a geometry)
var geometry = new THREE.BoxGeometry(cubeLengths.width, cubeLengths.height, cubeLengths.depth);
//Material to apply to the cube (green)
var material = new THREE.MeshBasicMaterial({color: '#CD5C5C'});
//Applies material to BoxGeometry
var cube = new THREE.Mesh(geometry, material);

// water
var waterGeometry = new THREE.BoxGeometry(cubeWaterLengths.width, cubeWaterLengths.height, cubeWaterLengths.depth);
//Material to apply to the cube (green)
var waterMaterial = new THREE.MeshBasicMaterial({color: 'blue'});
//Applies material to BoxGeometry
var waterCube = new THREE.Mesh(waterGeometry, waterMaterial);

waterCube.material.transparent = IS_WATER_TRANSPARENT;
waterCube.material.opacity = WATER_TRANSPARENCY;

scene.add(waterCube);

const loader = new THREE.GLTFLoader();

// Load a glTF resource
loader.load(
    // resource URL
    // https://sketchfab.com/3d-models/cartoon-lowpoly-small-city-free-pack-edd1c604e1e045a0a2a552ddd9a293e6
    '/StaticFiles/3d-model/scene.gltf',
    // called when the resource is loaded
    function (gltf) {
        // scale 3d model
        gltf.scene.scale.x = .0009;
        gltf.scene.scale.y = .0022; //increased height scale for bigger body
        gltf.scene.scale.z = .0009;

        // position 3d model in middle
        gltf.scene.position.x = 0.4;
        gltf.scene.position.y = translateY;
        gltf.scene.position.z = .4;

        cube = gltf.scene;
        scene.add(gltf.scene);
    },
    // called while loading is progressing
    function (xhr) {
        console.log((xhr.loaded / xhr.total * 100) + '% loaded');
    },
    // called when loading has errors
    function (error) {
        console.log('An error happened');
    }
);

//Sets camera's distance away from cube (using this explanation only for simplicity's sake - in reality this actually sets the 'depth' of the camera's position)

camera.position.z = 6;
camera.position.y = 2;
camera.position.x = 5;
camera.rotation.y = .75;

//Rendering

function render() {
    requestAnimationFrame(render);
    renderer.render(scene, camera);

    if (globals.cubeRotationZ
        && globals.cubeRotationX) {
        // update rotation of y-axis
        if (cube.rotation.z < globals.cubeRotationZ) {
            cube.rotation.z += ROTATION_STEP;
        }

        if (cube.rotation.z > globals.cubeRotationZ) {
            cube.rotation.z -= ROTATION_STEP;
        }

        // update rotation of x-axis
        if (cube.rotation.x < globals.cubeRotationX) {
            cube.rotation.x += ROTATION_STEP;
        }

        if (cube.rotation.x > globals.cubeRotationX) {
            cube.rotation.x -= ROTATION_STEP;
        }

        // 400mm maxHeight => 100%
        // 0.18 max-y-axis addition => 100%
        // calculate percent for height
        var percentHeightIncreaseFromZero = currentHeight / 400 * 100;
        var newTranslateYValue = .62 + 0.18 / 100 * percentHeightIncreaseFromZero;
        
        // update height of model
        if (translateY < newTranslateYValue) {
            translateYCube(0.0001);
        }

        if (translateY > newTranslateYValue) {
            translateYCube(0.0001);
        }
    }

    if (IS_ROTATING) {
        scene.rotation.y += AUTO_ROTATION_STEP;
    }
}

render();

function getRadiantFromDegree(degree) {
    return degree * Math.PI / 180;
}

function getDegreeFromRadiant(radiant) {
    return radiant * 180 / Math.PI;
}

function toggleHelpers(isShowing) {
    if (isShowing === true) {
        cubeAxis = new THREE.AxesHelper(200);
        cube.add(cubeAxis);

        plane = new THREE.GridHelper(100, 100, 'aqua', 'aqua');
        scene.add(plane);
    } else {
        cube.remove(cubeAxis);
        scene.remove(plane);
    }
}

function translateYCube(value) {
    cube.translateY(value);
    translateY += value;
}