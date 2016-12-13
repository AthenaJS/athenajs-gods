import { Scene, Text, Sprite, Menu, AudioManager as AM, EventManager as EM, ResourceManager as RM } from 'athenajs';
import GodsMap from 'maps/GodsMap';
import sceneHud from 'scenes/sceneHud';

class godsLevel1 extends Scene {
    constructor() {
        super({
            name: 'level1',
            hudScene: sceneHud,
            resources: [
                // images
                {
                    id: 'sprites',
                    type: 'image',
                    src: 'gods/img/sprites_blue.png'
                }, {
                    id: 'tiles',
                    type: 'image',
                    src: 'gods/img/gods_sprites_tiles_level1.png'
                }, {
                    id: 'objects',
                    type: 'image',
                    src: 'gods/img/gods_objects.png'
                }, {
                    id: 'enemies',
                    type: 'image',
                    src: 'gods/img/godsSpritesLevel1.png'
                }, {
                    id: 'font',
                    type: 'image',
                    src: 'gods/img/bitmapFont.png'
                },

                // maps
                {
                    id: 'mapLevel1',
                    type: 'map',
                    src: 'gods/js/maps/mapLevel1.json'
                },

                // objects
                {
                    id: 'Gem',
                    type: 'script',
                    src: 'sprites/Gem'
                },/* {
                    id: 'spear',
                    type: 'script',
                    src: 'gods/js/sprites/Spear.js',
                    poolSize: 20
                }, {
                    id: 'spear-wood',
                    type: 'script',
                    src: 'gods/js/sprites/SpearWood.js'
                }, {
                    id: 'smallItem',
                    type: 'script',
                    src: 'gods/js/sprites/SmallItem.js'
                }, {
                    id: 'weapon',
                    type: 'script',
                    src: 'gods/js/sprites/Weapon.js',
                    poolSize: 20
                }, {
                    id: 'enemy1',
                    type: 'script',
                    src: 'gods/js/sprites/Enemy1.js',
                    poolSize: 20
                }, {
                    id: 'flying-enemy1',
                    type: 'script',
                    src: 'gods/js/sprites/FlyingEnemy1.js',
                    poolSize: 5
                }, {
                    id: 'switch',
                    type: 'script',
                    src: 'gods/js/sprites/Switch.js'
                }, {
                    id: 'gods',
                    type: 'script',
                    src: 'gods/js/sprites/GodsSprite.js'
                }, {
                    id: 'death_explosion',
                    type: 'script',
                    src: 'gods/js/sprites/DeathExplosion.js'
                }, {
                    id: 'enemy_explosion',
                    type: 'script',
                    src: 'gods/js/sprites/EnemyExplosion.js'
                }, {
                    id: 'moving_platform',
                    type: 'script',
                    src: 'gods/js/sprites/MovingPlatform.js'
                },*/

                // bitmap fonts
                // {
                //     id: 'BitmapFont',
                //     type: 'script',
                //     src: 'gods/js/sprites/BitmapFont.js'
                // },

                // sound
                /*
                {
                    id: 'step_left',
                    type: 'audio',
                    src: 'gods/audio/step/step_left.wav'
                }, {
                    id: 'step_right',
                    type: 'audio',
                    src: 'gods/audio/step/step_right.wav'
                }, {
                    id: 'jump',
                    type: 'audio',
                    src: 'gods/audio/jump.wav'
                }, {
                    id: 'restart',
                    type: 'audio',
                    src: 'gods/audio/restart.wav'
                }, {
                    id: 'appearLeft',
                    type: 'audio',
                    src: 'gods/audio/hostile_appearing_left/mid2.wav'
                }, {
                    id: 'bounce',
                    type: 'audio',
                    src: 'gods/audio/bounce/1.wav'
                }, {
                    id: 'hit2',
                    type: 'audio',
                    src: 'gods/audio/hero_hurt/hurt_2.wav'
                }, {
                    id: 'explode1',
                    type: 'audio',
                    src: 'gods/audio/explosion_ground/explosion_ground_1.wav'
                }, {
                    id: 'explode2',
                    type: 'audio',
                    src: 'gods/audio/explosion_ground/explosion_ground_2.wav'
                }, {
                    id: 'land',
                    type: 'audio',
                    src: 'gods/audio/land.wav'
                }, {
                    id: 'death',
                    type: 'audio',
                    src: 'gods/audio/hero_death.wav'
                }, {
                    id: 'spike',
                    type: 'audio',
                    src: 'gods/audio/spike.wav'
                }, {
                    id: 'weapon_throw',
                    type: 'audio',
                    src: 'gods/audio/weapon_throw.wav'
                }, {
                    id: 'leverActivated',
                    type: 'audio',
                    src: 'gods/audio/lever_activated.wav'
                }, {
                    id: 'take_bonus',
                    type: 'audio',
                    src: 'gods/audio/take_bonus.wav'
                }, {
                    id: 'weapon_crash',
                    type: 'audio',
                    src: 'gods/audio/weapon_crash.wav'
                }*/
            ]
        });
    }

    onLoad() {
        console.log('[sceneLevel1] onLoad');

        super.onLoad();

        var that = this;

        console.log('[scene ' + this.name + '] ' + 'onLoad');

        this.setMap(new GodsMap(RM.getResourceById('mapLevel1')));

        // this.setMap(RM.getResourceById('mapLevel1'));
    }

    pause() {
        this.pauseText.show();
        super.pause();
    }

    unpause() {
        console.log('unpause');
        this.pauseText.hide();
        super.unpause();
    }
    start(doNotResetMap) {
        EM.clearEvents();

        EM.installKeyCallback('ESCAPE', 'up', () => {
            EM.clearEvents();

            this.animate('Fade', {
                startValue: 1,
                endValue: 0
            }).then(() => {
                this.notify('game:exitLevel');
            });
        });

        AM.play('restart');

        this.hudScene.setOpacity(0);
        this.setOpacity(1);

        this.animate('Mosaic', {
            when: 'post',
            duration: 800,
            startValue: 0.00005,
            endValue: 0.3,
            easing: 'linear'
        });

        super.start(doNotResetMap);

        /*                this.pauseText = new Text('pause', {
                    text: 'P A U S E',
                    color: 'white',
                    fontSize: '25px'
                }).moveTo(200, 300).hide();*/

        // this.map.addObject(new(RM.getResourceById('MovingPlatform'))({
        //     x: 420,
        //     y: 490
        // }));

        // var text = require('sprites/BitmapFont').default;
        var text = RM.getResourceById('BitmapFont');

        this.pauseText = new text('infoTxt', {
            size: 'big',
            w: 180,
            h: 32,
            visible: false,
            scrollOffsetX: 0,
            scrollOffsetY: 0,
            text: 'pause'
        });

        this.addObject(this.pauseText);

        this.pauseText.center();

        this.pauseText.y -= 100;

        this.bindEvents('game:restart');
    }
    onEvent(event) {
        if (event.type === 'game:restart') {
            setTimeout(() => {
                this.stop();
                this.resume();
            }, 0);
        }
    }

    stop() {
        EM.clearEvents();

        super.stop();
    }

    debug() {
        this.map.debug(!this.map.isDebug);
    }
};

export default new godsLevel1();