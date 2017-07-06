/*jshint esversion: 6*/
import { BitmapText, ResourceManager as RM } from 'athenajs';

class BitmapFont extends BitmapText {
    constructor(type, options) {
        super(type, Object.assign(options, {
            offsetX: 34,
            bmStartY: 36,
            charWidth: 16,
            charHeight: 18,
            imageSrc: 'font'
        }));
        // options = options || {};

        // var size = options.size || 'small';

        // options.imageSrc = 'font';

        // if (size === 'small') {
        // 	options.offsetX = 34;
        // 	options.bmStartY = 36,
        // 	options.charWidth = 16;
        // 	options.charHeight = 18;
        // } else {
        // 	// TODO
        // 	options.offsetX = 34;
        // 	options.bmStartY = 2,
        // 	options.charWidth = 32;
        // 	options.charHeight = 32;
        //   options.lineSpacing = 3;
        //   options.letterSpacing = 4;
        // }

        // // SUPERHEREthis._super(type, options);
    }
}

RM.registerScript('BitmapFont', BitmapFont);

export default BitmapFont;