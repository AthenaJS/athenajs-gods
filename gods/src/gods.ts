import SceneIntro from './scenes/SceneIntro';
import SceneLevel1 from './scenes/SceneLevel1';
import SceneMenu from './scenes/sceneMenu';

import { Game, GameOptions, GameEvent, ResourceManager as RM } from 'athenajs';

/* hardcoded import every sprite */
import BitmapFont from './sprites/BitmapFont';
RM.registerScript('BitmapFont', BitmapFont);
import DeathExplosion from './sprites/DeathExplosion';
RM.registerScript('DeathExplosion', DeathExplosion);
import Enemy1 from './sprites/Enemy1';
RM.registerScript('Enemy1', Enemy1);
import EnemyExplosion from './sprites/EnemyExplosion';
RM.registerScript('EnemyExplosion', EnemyExplosion);
import FlyingEnemy1 from './sprites/FlyingEnemy1';
RM.registerScript('FlyingEnemy1', FlyingEnemy1);
import Gem from './sprites/Gem';
RM.registerScript('Gem', Gem);
import GodsSprite from './sprites/GodsSprite';
RM.registerScript('GodsSprite', GodsSprite);
import LifeMetter from './sprites/LifeMetter';
RM.registerScript('LifeMetter', LifeMetter);
import MovingPlatform from './sprites/MovingPlatform';
RM.registerScript('MovingPlatform', MovingPlatform);
import SmallItem from './sprites/Smallitem';
RM.registerScript('SmallItem', SmallItem);
import Spear from './sprites/Spear';
RM.registerScript('Spear', Spear);
import SpearWood from './sprites/SpearWood';
RM.registerScript('SpearWood', SpearWood);
import Switch from './sprites/Switch';
RM.registerScript('Switch', Switch);
import Weapon from './sprites/Weapon';
RM.registerScript('Weapon', Weapon);


/* Automatically append every sprite files to the Gods build: since there's no way to dynamically load
   scripts when using Webpack we tell it to add every sprite to our build.
 */
// var req = require.context("./sprites/", false, /^(.*\.(js$))[^.]*$/igm);
// req.keys().forEach(function (key) {
//     req(key);
// });

const sceneMenu: SceneMenu = new SceneMenu(),
    sceneLevel1:SceneLevel1 = new SceneLevel1(),
    sceneIntro: SceneIntro = new SceneIntro();

/*jshint devel: true*/
class GodsClass extends Game {
    currentLevel: number;

    constructor(options:GameOptions) {
        super(options);
        // intro
        this.currentLevel = -1;
    }

    onEvent(event: GameEvent) {
        switch (event.type) {
            case 'game:gotoMenu':
                this.setScene(sceneMenu);
                break;

            case 'game:startGame':
                this.setScene(sceneLevel1);
                break;

            case 'game:gameover':
            case 'game:exitLevel':
                // this.stopScene();
                this.setScene(sceneIntro);
                break;
        }
    }
};

const gods:Game = new GodsClass({
    debug: true,
    name: 'Gods',
    target: '.main',
    showFps: true,
    width: 1024,
    height: 768,
    sound: true
});

gods.setScene(sceneIntro);

// debug stuff
document.body.addEventListener('keyup', (event) => {
    // if (event.keyCode === 82) {
    //     gods.scene.stop();

    //     gods.scene.resume();
    // }

    if (event.keyCode === 83) {
        gods.toggleSound(!gods.sound);
    }

    if (event.keyCode === 72) {
        if (gods.scene.hudScene) {
            var opacity = gods.scene.hudScene.getOpacity();
            gods.scene.hudScene.setOpacity(opacity ? 0 : 1);
        }
    }
});