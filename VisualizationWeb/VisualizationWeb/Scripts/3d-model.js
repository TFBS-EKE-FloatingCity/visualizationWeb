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

var translateYValue = 2;

var height = $('#CityRotationChartPanel').width(); // window.innerWidth / 2;
var width = $('#CityRotationChartPanel').width(); // window.innerWidth / 2;

//Creates scene and camera

var scene = new THREE.Scene();
scene.background = new THREE.Color('#fff');
// LIGHTS
var light = new THREE.AmbientLight(0xffffff, 2);
scene.add(light);

var light2 = new THREE.PointLight(0xffffff, 0.5);
scene.add(light2);
var camera = new THREE.PerspectiveCamera(75, width / height, 0.1, 1000);

//Creates renderer and adds it to the DOM

var renderer,
    cubeAxis,
    plane,
    myCanvas = document.getElementById('3d-model');

var shouldRotate = false;

var controls = new THREE.OrbitControls(camera, myCanvas);

// RENDERER
renderer = new THREE.WebGLRenderer({canvas: myCanvas, antialias: true});
renderer.setSize(width, height);

//The Box!

//BoxGeometry (makes a geometry)
var geometry = new THREE.BoxGeometry(cubeLengths.width, cubeLengths.height, cubeLengths.depth);
//Material to apply to the cube (green)
var material = new THREE.MeshBasicMaterial({color: '#CD5C5C'});
//Applies material to BoxGeometry
var cube = new THREE.Mesh(geometry, material);
//Adds cube to the scene
// scene.add(cube);

translateYCube(cubeLengths.height);

// water
var waterGeometry = new THREE.BoxGeometry(cubeWaterLengths.width, cubeWaterLengths.height, cubeWaterLengths.depth);
//Material to apply to the cube (green)
var waterMaterial = new THREE.MeshBasicMaterial({color: 'blue'});
//Applies material to BoxGeometry
var waterCube = new THREE.Mesh(waterGeometry, waterMaterial);

// enable transparency
waterCube.material.transparent = true;
// set opacity to 50%
waterCube.material.opacity = 0.5;

scene.add(waterCube);

const loader = new THREE.GLTFLoader();

// Load a glTF resource
loader.load(
    // resource URL
    '/StaticFiles/3d-model/scene.gltf',
    // called when the resource is loaded
    function (gltf) {
        gltf.scene.scale.x = .0009;
        gltf.scene.scale.y = .0009;
        gltf.scene.scale.z = .0009;

        gltf.scene.position.x = 0.4;
        gltf.scene.position.y = .7;
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

    if (cubeRotationZ
        && cubeRotationX) {
        cube.rotation.z = cubeRotationZ;
        cube.rotation.x = cubeRotationX;
        // cube.translateY(newHeight - oldHeight); TODO: fix height values
    }

    if (shouldRotate) {
        scene.rotation.y += 0.01;
    }
}

render();

// updateModelRotation(sensors);

function updateModelRotation2(sensors) {
    var radiant;

    if (sensors.a === sensors.b) {
        // sensors a and b are even
        cube.rotation.z = +0.0;
    }

    if (sensors.a === sensors.c) {
        // sensors a and c are even
        cube.rotation.x = +0.0;
    }

    if (sensors.b > sensors.a) {
        // B > A
        radiant = (-1) * Math.atan((sensors.b - sensors.a) / cubeLengths.width);
        cube.rotation.z = radiant;
    }

    if (sensors.a > sensors.b) {
        // A > B
        radiant = Math.atan((sensors.a - sensors.b) / cubeLengths.width);
        cube.rotation.z = radiant;
    }

    if (sensors.a > sensors.c) {
        // A > C
        radiant = (-1) * Math.atan((sensors.a - sensors.c) / cubeLengths.width);
        cube.rotation.x = radiant;
    }

    if (sensors.c > sensors.a) {
        // C > A
        radiant = Math.atan((sensors.c - sensors.a) / cubeLengths.width);
        cube.rotation.x = radiant;
    }

    // set height
    var heights = {
        a: sensors.a * Math.cos(Math.abs(cube.rotation.z)),
        b: sensors.b * Math.cos(Math.abs(cube.rotation.z)),
        c: sensors.c * Math.cos(Math.abs(cube.rotation.x)),
    };

    var newHeight = (heights.a + heights.b + heights.c) / 3 + cubeLengths.height;

    cube.translateY(newHeight - translateYValue);
    translateYValue = newHeight;

    updateForms();
}

function getRadiantFromDegree(degree) {
    return degree * Math.PI / 180;
}

function getDegreeFromRadiant(radiant) {
    return radiant * 180 / Math.PI;
}

function toggleHelpers(shouldShow) {
    if (shouldShow === true) {
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
    translateYValue += value;
}