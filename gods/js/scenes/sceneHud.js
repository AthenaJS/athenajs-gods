import { Scene, Pool, ResourceManager as RM } from 'athenajs';
/*
import Scene from 'Scene/Scene';
import Pool from 'Util/Pool';
import ResourceManager from 'Resource/ResourceManager';
*/

class GodsHudScene extends Scene {
    constructor(options = {}) {
        super({
            name: 'godsHud',
            resources: [{
                id: 'objects',
                type: 'image',
                src: 'gods/img/gods_objects.png'
            },
            // objects
            // { id: 'life_metter', type: 'script', src: 'gods/js/sprites/LifeMetter.js' },
            // { id: 'life_metter_mask', type: 'script', src: 'gods/js/sprites/LifeMetterMask.js' },
            {
                id: 'font',
                type: 'image',
                src: 'gods/img/bitmapFont.png'
            },
                // { id: 'BitmapFont', type: 'script', src: 'gods/js/sprites/BitmapFont.js' }
            ]
        });

        this.inventory = new Array(3);

        this.setOptions();

        // listen to player specific events
        this.bindEvents('player:hit player:coins player:item player:death game:message');

        // TODO: there is: reset() which resets everything to default
        // AND (TODO!) restart() which resets only some parts
        this.lives = 3;
        this.points = 0;
        this.money = 0;
        this.weapon = null;

        this.lifeSprites = [];
    }
    resetEnergy() {
        this.energy = this.maxEnergy;
        // back to full energy
        this.lifeMetter.resetEnergy();
    }

    setOptions(options) {
        options = options || {};

        this.messages = options.messages || [];
        this.maxEnergy = options.maxEnergy || 10;
        this.maxLives = options.maxLives || 10;
    }

    // we listen for player hit, points,...
    onEvent(event) {
        if (!this.running) {
            return;
        }

        if (event.type === 'player:hit') {
            this.lifeMetter.updateMetterHeight(event.data.damage);
        } else if (event.type === 'player:death') {
            if (--this.lives < 1) {
                this.notify('game:gameover');
            } else {
                console.log('notify lost live');
                this.notify('game:restart');
            }
            // hide one life sprite
            this.lifeSprites[this.lives].visible = false;
        } else if (event.type === 'game:message') {
            // console.log('sceneHud: got message', event.data.message);
            this.info.setText(event.data.message);
        }
    }

    start() {
        // console.log('[sceneHud] Starting Scene!');
        // super.start();

        this.resetEnergy();

        // this.addHudElements();
    }

    setup() {
        /*
        var LifeMetter = require("sprites/LifeMetter").default,
            Lives = require('sprites/SmallItem').default;
        */
        var LifeMetter = RM.getResourceById('LifeMetter'),
            Lives = RM.getResourceById('SmallItem');

        // add life metter
        this.lifeMetter = new LifeMetter({
            x: 32,
            y: 544,
            maxEnergy: this.maxEnergy
        });

        this.addObject(this.lifeMetter);

        // add lives
        for (var i = 0; i < this.lives; i++) {
            this.lifeSprites.push(new Lives({
                data: {
                    itemType: 'life',
                },
                x: 31 * i,
                y: 0
            }))
            this.addObject(this.lifeSprites[i]);
        }

        // add info element
        // var SimpleText = require('sprites/BitmapFont').default;
        var Text = RM.getResourceById('BitmapFont');

        this.info = new Text('infoTxt', {
            width: 400,
            height: 18,
            x: 300,
            y: 640,
            scrollOffsetX: 0,
            scrollOffsetY: 0
        });

        // info.visible = false;

        this.addObject(this.info);

    }
};

export default new GodsHudScene();