import GodsMapEvent from './GodsMapEvent';


    var map = {
        src: 'tiles',
        // mapEventClass: GodsMapEvent,
        viewportX: 0,
        viewportY: 0,
        viewportW: 1024,
        viewportH: 768,	/* 736 == 23 tiles */
        width: 2112,
        height: 768,	/* 736 */
        tileWidth: 64,
        tileHeight: 32,
        name: 'level1',
        startX: 125,
        startY: 448,
        // tiles: unsigned char
        // 255 = noTile
        // mapUrl: 'maps/map1.bin'
        dataUrl: 'gods/data/mapLevel1.bin',
        map: null,
        objects: null,
        // map: [
		// 	   119, 7,   8,   9,   9,   9,   9,   9,   9,   9,   9,   9,   9,   9,   9,   15,  9,   132, 130, 255,  255,  0, 0, 1, 1, 1, 1, 255, 1, 0, 1, 0, 11,
        //     119, 7,   8,   9,   9,   9,   58,  54,  54,  54,  28,  9,   9,   122, 124, 31,  9,   118, 130, 255,  255,  0, 0, 1, 1, 1, 1, 255, 1, 0, 1, 0, 11,
        //     116, 116, 116, 117, 32,  9,   83,  142, 142, 142, 29,  9,   9,   132, 134, 8,   9,   132, 130, 255,  255,  0, 0, 1, 1, 1, 1, 255, 1, 0, 1, 0, 11,
        //     119, 114, 6,   6,   8,   9,   115, 116, 116, 116, 117, 32,  9,   33,  6,   8,   9,   118, 74,  255,  255,  0, 0, 1, 1, 1, 1, 255, 1, 0, 1, 0, 11,
        //     119, 7,   10,  10,  11,  8,   33,  6,   6,   6,   6,   8,   9,   9,   9,   9,   9,   132, 132, 255,  255,  0, 0, 1, 1, 1, 1, 255, 1, 0, 1, 0, 11,
        //     119, 122, 123, 123, 124, 32,  55,  9,   55,  9,   55,  9,   9,   9,   55,  9,   9,   132, 120, 255,  255,  0, 0, 1, 1, 1, 1, 255, 1, 0, 1, 0, 11,
        //     119, 132, 133, 133, 134, 8,   56,  9,   56,  9,   56,  9,   9,   9,   56,  9,   49,  118, 119, 255,  255,  0, 0, 1, 1, 1, 1, 255, 1, 0, 1, 0, 11,
        //     119, 114, 6,   6,   6,   115, 116, 116, 116, 116, 116, 116, 117, 32,  9,   9,   15,  132, 119, 255,  255,  0, 0, 1, 1, 1, 1, 255, 1, 0, 1, 0, 11,
        //     119, 7,   10,  10,  11,  6,   6,   6,   6,   6,   101, 6,   6,   8,   9,   9,   15,  118, 119, 255,  255,  0, 0, 1, 1, 1, 1, 255, 1, 0, 1, 0, 11,
        //     119, 25,  26,  28,  33,  11,  11,  11,  11,  11,  121, 10,  11,  122, 123, 124, 31,  132, 119, 255,  255,  0, 0, 1, 1, 1, 1, 255, 1, 0, 1, 0, 11,
        //     119, 140, 141, 29,  34,  74,  32,  49,  9,   55,  131, 8,   9,   132, 133, 134, 41,  118, 119, 255,  255,  0, 0, 1, 1, 1, 1, 255, 1, 0, 1, 0, 11,
        //     119, 140, 141, 29,  9,   90,  32,  15,  9,   56,  131, 8,   9,   132, 133, 134, 41,  132, 119, 255,  255,  0, 0, 1, 1, 1, 1, 255, 1, 0, 1, 0, 11,
        //     116, 116, 116, 116, 117, 6,   32,  15,  122, 123, 123, 124, 32,  132, 133, 134, 41,  118, 119, 255,  255,  0, 0, 1, 1, 1, 1, 255, 1, 0, 1, 0, 11,
        //     119, 114, 6,   6,   6,   8,   9,   15,  132, 133, 133, 119, 8,   34,   6,   6,  40,  132, 119, 255,  255,  0, 0, 1, 1, 1, 1, 255, 1, 0, 1, 0, 11,
        //     119, 25,  26,  54,  28,  9,   9,   15,  33,  6,   84,  86,  8,   9,   33,   11, 15,  118, 119, 255,  255,  0, 0, 1, 1, 1, 1, 255, 1, 0, 1, 0, 11,
        //     119, 140, 141, 142, 29,  9,   9,   15,  34,  6,   85,  87,  8,   58,  54,   28, 15,  132, 119, 255,  255,  0,   0,  1, 1, 1, 1, 255, 1, 0, 1, 0, 11,
        //     119, 140, 141, 142, 29,  9,   9,   15,  9,   9,   85,  87,  8,   83,  142,  29, 15,  118, 119, 255,  255,  0, 0, 1, 1, 1, 1, 255, 1, 0, 1, 0, 11,
        //     123, 123, 123, 123, 123, 123, 123, 123, 123, 123, 123, 123, 123, 123, 123, 123, 123, 123, 123, 123, 123, 0, 0, 1, 1, 1, 1, 255, 1, 0, 1, 0, 11,
        //     133, 133, 133, 133, 133, 133, 133, 133, 133, 133, 133, 133, 133, 133, 133, 133, 133, 133, 133, 133, 133, 0, 0, 1, 1, 1, 1, 255, 1, 0, 1, 0, 11,
        //     255,  255,  255,  255,  255,  255,  255,  255,  255,  255,  255,  255,  255,  255,  255,  255,  255,  255,  255,  255,  255,  255, 0, 1, 1, 1,  9, 255, 1, 0, 1, 0, 11,
        //     255,  255,  255,  255,  255,  255,  255,  255,  255,  255,  255,  255,  255,  255,  255,  255,  255,  255,  255,  255,  255,  255, 0, 1, 1, 1,  9, 255, 1, 0, 1, 0, 11,
        //     255,  255,  255,  255,  255,  255,  255,  255,  255,  255,  255,  255,  255,  255,  255,  255,  255,  255,  255,  255,  255,  255, 0, 1, 1, 1,  9, 255, 1, 0, 1, 0, 11,
        //     255,  255,  255,  255,  255,  255,  255,  255,  255,  255,  255,  255,  255,  255,  255,  255,  255,  255,  255,  255,  255,  255, 0, 1, 1, 1,  9, 255, 1, 0, 1, 0, 11,
        //     255,  255,  255,  255,  255,  255,  255,  255,  255,  255,  255,  255,  255,  255,  255,  255,  255,  255,  255,  255,  255,  255, 0, 1, 1, 1,  9, 255, 1, 0, 1, 0, 11
        // ],
        // // unsigned char
        // objects: [
        //     1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        //     1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        //     1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        //     1, 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        //     1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        //     1, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        //     1, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        //     1, 0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        //     1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        //     1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 2, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        //     1, 0, 0, 0, 0, 1, 0, 2, 0, 0, 0, 0, 0, 1, 1, 1, 2, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        //     1, 0, 0, 0, 0, 1, 0, 2, 0, 0, 0, 0, 0, 1, 1, 1, 2, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        //     1, 1, 1, 1, 1, 0, 0, 2, 1, 1, 1, 1, 0, 1, 1, 1, 2, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        //     1, 0, 0, 0, 0, 0, 0, 2, 1, 1, 1, 1, 0, 0, 0, 0, 2, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        //     1, 0, 0, 0, 0, 0, 0, 2, 0, 0, 1, 1, 0, 0, 0, 0, 2, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        //     1, 0, 0, 0, 0, 0, 0, 2, 0, 0, 1, 1, 0, 0, 0, 0, 2, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        //     1, 0, 0, 0, 0, 0, 0, 2, 0, 0, 1, 1, 0, 0, 0, 0, 2, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        //     1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        //     1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        //     1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        //     1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        //     1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        //     1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        //     1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0
        // ],
		triggers: {

		},
		windows: {

		},
        sprites: [
            // {
            //     x: 0,
            //     y: 0,
            //     spriteId: 'enemy1'
            // },
            // {
            //     x: 300,
            //     y: 300,
            //     spriteId: 'enemy2'
            // }
        ],
        tiles: [
            {
                offsetX: 122,
                offsetY: 199,
                width: 64,
                height: 32,
                canWalk: 0		// 0 == yes
            },
            {
                offsetX: 188,
                offsetY: 199,
                width: 64,
                height: 32,
                canWalk: 0		// 0 == yes
            },
            // line 2
            {
                offsetX: 56,
                offsetY: 233,
                width: 64,
                height: 32,
                canWalk: 0		// 0 == yes
            },
            {
                offsetX: 122,
                offsetY: 233,
                width: 64,
                height: 32,
                canWalk: 0		// 0 == yes
            },
            {
                offsetX: 188,
                offsetY: 233,
                width: 64,
                height: 32,
                canWalk: 0		// 0 == yes
            },
            {
                offsetX: 254,
                offsetY: 233,
                width: 64,
                height: 32,
                canWalk: 0		// 0 == yes
            },
            {
                offsetX: 320,
                offsetY: 233,
                width: 64,
                height: 32,
                canWalk: 0		// 0 = false
            },
            {
                offsetX: 386,
                offsetY: 233,
                width: 64,
                height: 32,
                canWalk: 0		// 0 == yes
            },
            {
                offsetX: 452,
                offsetY: 233,
                width: 64,
                height: 32,
                canWalk: 0		// 0 == yes
            },
            {
                offsetX: 518,
                offsetY: 233,
                width: 64,
                height: 32,
                canWalk: 0		// 0 == yes
            },
            {
                offsetX: 584,
                offsetY: 233,
                width: 64,
                height: 32,
                canWalk: 0		// 0 == yes
            },
            {
                offsetX: 650,
                offsetY: 233,
                width: 64,
                height: 32,
                canWalk: 0		// 0 == yes
            },
            {
                offsetX: 716,
                offsetY: 233,
                width: 64,
                height: 32,
                canWalk: 0		// 0 == yes
            },
            {
                offsetX: 782,
                offsetY: 233,
                width: 64,
                height: 32,
                canWalk: 0		// 0 == yes
            },
            {
                offsetX: 848,
                offsetY: 233,
                width: 64,
                height: 32,
                canWalk: 0		// 0 == yes
            },
            {
                offsetX: 914,
                offsetY: 233,
                width: 64,
                height: 32,
                canWalk: 0		// 0 == yes
            },
            {
                offsetX: 980,
                offsetY: 233,
                width: 64,
                height: 32,
                canWalk: 0		// 0 == yes
            },
            {
                offsetX: 1046,
                offsetY: 233,
                width: 64,
                height: 32,
                canWalk: 0		// 0 == yes
            },
            {
                offsetX: 1112,
                offsetY: 233,
                width: 64,
                height: 32,
                canWalk: 0		// 0 == yes
            },
            {
                offsetX: 1178,
                offsetY: 233,
                width: 64,
                height: 32,
                canWalk: 0		// 0 == yes
            },
            {
                offsetX: 1244,
                offsetY: 233,
                width: 64,
                height: 32,
                canWalk: 0		// 0 == yes
            },
            {
                offsetX: 1310,
                offsetY: 233,
                width: 64,
                height: 32,
                canWalk: 0		// 0 == yes
            },
            {
                offsetX: 1376,
                offsetY: 233,
                width: 64,
                height: 32,
                canWalk: 0		// 0 == yes
            },
            {
                offsetX: 1442,
                offsetY: 233,
                width: 64,
                height: 32,
                canWalk: 0		// 0 == yes
            },
            {
                offsetX: 1508,
                offsetY: 233,
                width: 64,
                height: 32,
                canWalk: 0		// 0 == yes
            },
            {
                offsetX: 1574,
                offsetY: 233,
                width: 64,
                height: 32,
                canWalk: 0		// 0 == yes
            },
            {
                offsetX: 1640,
                offsetY: 233,
                width: 64,
                height: 32,
                canWalk: 0		// 0 == yes
            },
            {
                offsetX: 1706,
                offsetY: 233,
                width: 64,
                height: 32,
                canWalk: 0		// 0 == yes
            },
            {
                offsetX: 1772,
                offsetY: 233,
                width: 64,
                height: 32,
                canWalk: 0		// 0 == yes
            },
            {
                offsetX: 1838,
                offsetY: 233,
                width: 64,
                height: 32,
                canWalk: 0		// 0 == yes
            },
            {
                offsetX: 1904,
                offsetY: 233,
                width: 64,
                height: 32,
                canWalk: 0		// 0 == yes
            },
            // 3rd line
            {
                offsetX: 56,
                offsetY: 267,
                width: 64,
                height: 32,
                canWalk: 0		// 0 == yes
            },
            {
                offsetX: 122,
                offsetY: 267,
                width: 64,
                height: 32,
                canWalk: 0		// 0 == yes
            },
            {
                offsetX: 188,
                offsetY: 267,
                width: 64,
                height: 32,
                canWalk: 0		// 0 == yes
            },
            {
                offsetX: 254,
                offsetY: 267,
                width: 64,
                height: 32,
                canWalk: 0		// 0 == yes
            },
            {
                offsetX: 320,
                offsetY: 267,
                width: 64,
                height: 32,
                canWalk: 0		// 0 = false
            },
            {
                offsetX: 386,
                offsetY: 267,
                width: 64,
                height: 32,
                canWalk: 0		// 0 == yes
            },
            {
                offsetX: 452,
                offsetY: 267,
                width: 64,
                height: 32,
                canWalk: 0		// 0 == yes
            },
            {
                offsetX: 518,
                offsetY: 267,
                width: 64,
                height: 32,
                canWalk: 0		// 0 == yes
            },
            {
                offsetX: 584,
                offsetY: 267,
                width: 64,
                height: 32,
                canWalk: 0		// 0 == yes
            },
            {
                offsetX: 650,
                offsetY: 267,
                width: 64,
                height: 32,
                canWalk: 0		// 0 == yes
            },
            {
                offsetX: 716,
                offsetY: 267,
                width: 64,
                height: 32,
                canWalk: 0		// 0 == yes
            },
            {
                offsetX: 782,
                offsetY: 267,
                width: 64,
                height: 32,
                canWalk: 0		// 0 == yes
            },
            {
                offsetX: 848,
                offsetY: 267,
                width: 64,
                height: 32,
                canWalk: 0		// 0 == yes
            },
            {
                offsetX: 914,
                offsetY: 267,
                width: 64,
                height: 32,
                canWalk: 0		// 0 == yes
            },
            {
                offsetX: 980,
                offsetY: 267,
                width: 64,
                height: 32,
                canWalk: 0		// 0 == yes
            },
            {
                offsetX: 1046,
                offsetY: 267,
                width: 64,
                height: 32,
                canWalk: 0		// 0 == yes
            },
            {
                offsetX: 1112,
                offsetY: 267,
                width: 64,
                height: 32,
                canWalk: 0		// 0 == yes
            },
            {
                offsetX: 1178,
                offsetY: 267,
                width: 64,
                height: 32,
                canWalk: 0		// 0 == yes
            },
            {
                offsetX: 1244,
                offsetY: 267,
                width: 64,
                height: 32,
                canWalk: 0		// 0 == yes
            },
            {
                offsetX: 1310,
                offsetY: 267,
                width: 64,
                height: 32,
                canWalk: 0		// 0 == yes
            },
            {
                offsetX: 1376,
                offsetY: 267,
                width: 64,
                height: 32,
                canWalk: 0		// 0 == yes
            },
            {
                offsetX: 1442,
                offsetY: 267,
                width: 64,
                height: 32,
                canWalk: 0		// 0 == yes
            },
            {
                offsetX: 1508,
                offsetY: 267,
                width: 64,
                height: 32,
                canWalk: 0		// 0 == yes
            },
            {
                offsetX: 1574,
                offsetY: 267,
                width: 64,
                height: 32,
                canWalk: 0		// 0 == yes
            },
            {
                offsetX: 1640,
                offsetY: 267,
                width: 64,
                height: 32,
                canWalk: 0		// 0 == yes
            },
            {
                offsetX: 1706,
                offsetY: 267,
                width: 64,
                height: 32,
                canWalk: 0		// 0 == yes
            },
            {
                offsetX: 1772,
                offsetY: 267,
                width: 64,
                height: 32,
                canWalk: 0		// 0 == yes
            },
            {
                offsetX: 1838,
                offsetY: 267,
                width: 64,
                height: 32,
                canWalk: 0		// 0 == yes
            },
            {
                offsetX: 1904,
                offsetY: 267,
                width: 64,
                height: 32,
                canWalk: 0		// 0 == yes
            },
            // line 4
            {
                offsetX: 56,
                offsetY: 301,
                width: 64,
                height: 32,
                canWalk: 0		// 0 == yes
            },
            {
                offsetX: 122,
                offsetY: 301,
                width: 64,
                height: 32,
                canWalk: 0		// 0 == yes
            },
            {
                offsetX: 188,
                offsetY: 301,
                width: 64,
                height: 32,
                canWalk: 0		// 0 == yes
            },
            {
                offsetX: 254,
                offsetY: 301,
                width: 64,
                height: 32,
                canWalk: 0		// 0 == yes
            },
            {
                offsetX: 320,
                offsetY: 301,
                width: 64,
                height: 32,
                canWalk: 0		// 0 = false
            },
            {
                offsetX: 386,
                offsetY: 301,
                width: 64,
                height: 32,
                canWalk: 0		// 0 == yes
            },
            {
                offsetX: 452,
                offsetY: 301,
                width: 64,
                height: 32,
                canWalk: 0		// 0 == yes
            },
            {
                offsetX: 518,
                offsetY: 301,
                width: 64,
                height: 32,
                canWalk: 0		// 0 == yes
            },
            {
                offsetX: 584,
                offsetY: 301,
                width: 64,
                height: 32,
                canWalk: 0		// 0 == yes
            },
            {
                offsetX: 650,
                offsetY: 301,
                width: 64,
                height: 32,
                canWalk: 0		// 0 == yes
            },
            {
                offsetX: 716,
                offsetY: 301,
                width: 64,
                height: 32,
                canWalk: 0		// 0 == yes
            },
            {
                offsetX: 782,
                offsetY: 301,
                width: 64,
                height: 32,
                canWalk: 0		// 0 == yes
            },
            {
                offsetX: 848,
                offsetY: 301,
                width: 64,
                height: 32,
                canWalk: 0		// 0 == yes
            },
            {
                offsetX: 914,
                offsetY: 301,
                width: 64,
                height: 32,
                canWalk: 0		// 0 == yes
            },
            {
                offsetX: 980,
                offsetY: 301,
                width: 64,
                height: 32,
                canWalk: 0		// 0 == yes
            },
            {
                offsetX: 1046,
                offsetY: 301,
                width: 64,
                height: 32,
                canWalk: 0		// 0 == yes
            },
            {
                offsetX: 1112,
                offsetY: 301,
                width: 64,
                height: 32,
                canWalk: 0		// 0 == yes
            },
            {
                offsetX: 1178,
                offsetY: 301,
                width: 64,
                height: 32,
                canWalk: 0		// 0 == yes
            },
            {
                offsetX: 1244,
                offsetY: 301,
                width: 64,
                height: 32,
                canWalk: 0		// 0 == yes
            },
            {
                offsetX: 1310,
                offsetY: 301,
                width: 64,
                height: 32,
                canWalk: 0		// 0 == yes
            },
            {
                offsetX: 1376,
                offsetY: 301,
                width: 64,
                height: 32,
                canWalk: 0		// 0 == yes
            },
            {
                offsetX: 1442,
                offsetY: 301,
                width: 64,
                height: 32,
                canWalk: 0		// 0 == yes
            },
            {
                offsetX: 1508,
                offsetY: 301,
                width: 64,
                height: 32,
                canWalk: 0		// 0 == yes
            },
            {
                offsetX: 1574,
                offsetY: 301,
                width: 64,
                height: 32,
                canWalk: 0		// 0 == yes
            },
            {
                offsetX: 1640,
                offsetY: 301,
                width: 64,
                height: 32,
                canWalk: 0		// 0 == yes
            },
            {
                offsetX: 1706,
                offsetY: 301,
                width: 64,
                height: 32,
                canWalk: 0		// 0 == yes
            },
            {
                offsetX: 1772,
                offsetY: 301,
                width: 64,
                height: 32,
                canWalk: 0		// 0 == yes
            },
            {
                offsetX: 1838,
                offsetY: 301,
                width: 64,
                height: 32,
                canWalk: 0		// 0 == yes
            },
            {
                offsetX: 1904,
                offsetY: 301,
                width: 64,
                height: 32,
                canWalk: 0		// 0 == yes
            },
            // line 5
            {
                offsetX: 56,
                offsetY: 335,
                width: 64,
                height: 32,
                canWalk: 0		// 0 == yes
            },
            {
                offsetX: 122,
                offsetY: 335,
                width: 64,
                height: 32,
                canWalk: 0		// 0 == yes
            },
            {
                offsetX: 188,
                offsetY: 335,
                width: 64,
                height: 32,
                canWalk: 0		// 0 == yes
            },
            {
                offsetX: 254,
                offsetY: 335,
                width: 64,
                height: 32,
                canWalk: 0		// 0 == yes
            },
            {
                offsetX: 320,
                offsetY: 335,
                width: 64,
                height: 32,
                canWalk: 0		// 0 = false
            },
            {
                offsetX: 386,
                offsetY: 335,
                width: 64,
                height: 32,
                canWalk: 0		// 0 == yes
            },
            {
                offsetX: 452,
                offsetY: 335,
                width: 64,
                height: 32,
                canWalk: 0		// 0 == yes
            },
            {
                offsetX: 518,
                offsetY: 335,
                width: 64,
                height: 32,
                canWalk: 0		// 0 == yes
            },
            {
                offsetX: 584,
                offsetY: 335,
                width: 64,
                height: 32,
                canWalk: 0		// 0 == yes
            },
            {
                offsetX: 650,
                offsetY: 335,
                width: 64,
                height: 32,
                canWalk: 0		// 0 == yes
            },
            {
                offsetX: 716,
                offsetY: 335,
                width: 64,
                height: 32,
                canWalk: 0		// 0 == yes
            },
            {
                offsetX: 782,
                offsetY: 335,
                width: 64,
                height: 32,
                canWalk: 0		// 0 == yes
            },
            {
                offsetX: 848,
                offsetY: 335,
                width: 64,
                height: 32,
                canWalk: 0		// 0 == yes
            },
            {
                offsetX: 914,
                offsetY: 335,
                width: 64,
                height: 32,
                canWalk: 0		// 0 == yes
            },
            {
                offsetX: 980,
                offsetY: 335,
                width: 64,
                height: 32,
                canWalk: 0		// 0 == yes
            },
            {
                offsetX: 1046,
                offsetY: 335,
                width: 64,
                height: 32,
                canWalk: 0		// 0 == yes
            },
            {
                offsetX: 1112,
                offsetY: 335,
                width: 64,
                height: 32,
                canWalk: 0		// 0 == yes
            },
            {
                offsetX: 1178,
                offsetY: 335,
                width: 64,
                height: 32,
                canWalk: 0		// 0 == yes
            },
            {
                offsetX: 1244,
                offsetY: 335,
                width: 64,
                height: 32,
                canWalk: 0		// 0 == yes
            },
            {
                offsetX: 1310,
                offsetY: 335,
                width: 64,
                height: 32,
                canWalk: 0		// 0 == yes
            },
            {
                offsetX: 1376,
                offsetY: 335,
                width: 64,
                height: 32,
                canWalk: 0		// 0 == yes
            },
            {
                offsetX: 1442,
                offsetY: 335,
                width: 64,
                height: 32,
                canWalk: 0		// 0 == yes
            },
            {
                offsetX: 1508,
                offsetY: 335,
                width: 64,
                height: 32,
                canWalk: 0		// 0 == yes
            },
            // line 5
            {
                offsetX: 56,
                offsetY: 369,
                width: 64,
                height: 32,
                canWalk: 0		// 0 == yes
            },
            {
                offsetX: 122,
                offsetY: 369,
                width: 64,
                height: 32,
                canWalk: 0		// 0 == yes
            },
            {
                offsetX: 188,
                offsetY: 369,
                width: 64,
                height: 32,
                canWalk: 0		// 0 == yes
            },
            {
                offsetX: 254,
                offsetY: 369,
                width: 64,
                height: 32,
                canWalk: 0		// 0 == yes
            },
            {
                offsetX: 320,
                offsetY: 369,
                width: 64,
                height: 32,
                canWalk: 0		// 0 = false
            },
            {
                offsetX: 386,
                offsetY: 369,
                width: 64,
                height: 32,
                canWalk: 0		// 0 == yes
            },
            {
                offsetX: 452,
                offsetY: 369,
                width: 64,
                height: 32,
                canWalk: 0		// 0 == yes
            },
            {
                offsetX: 518,
                offsetY: 369,
                width: 64,
                height: 32,
                canWalk: 0		// 0 == yes
            },
            {
                offsetX: 584,
                offsetY: 369,
                width: 64,
                height: 32,
                canWalk: 0		// 0 == yes
            },
            {
                offsetX: 650,
                offsetY: 369,
                width: 64,
                height: 32,
                canWalk: 0		// 0 == yes
            },
            {
                offsetX: 716,
                offsetY: 369,
                width: 64,
                height: 32,
                canWalk: 0		// 0 == yes
            },
            {
                offsetX: 782,
                offsetY: 369,
                width: 64,
                height: 32,
                canWalk: 0		// 0 == yes
            },
            {
                offsetX: 848,
                offsetY: 369,
                width: 64,
                height: 32,
                canWalk: 0		// 0 == yes
            },
            {
                offsetX: 914,
                offsetY: 369,
                width: 64,
                height: 32,
                canWalk: 0		// 0 == yes
            },
            {
                offsetX: 980,
                offsetY: 369,
                width: 64,
                height: 32,
                canWalk: 0		// 0 == yes
            },
            {
                offsetX: 1046,
                offsetY: 369,
                width: 64,
                height: 32,
                canWalk: 0		// 0 == yes
            },
            {
                offsetX: 1112,
                offsetY: 369,
                width: 64,
                height: 32,
                canWalk: 0		// 0 == yes
            },
            {
                offsetX: 1178,
                offsetY: 369,
                width: 64,
                height: 32,
                canWalk: 0		// 0 == yes
            },
            {
                offsetX: 1244,
                offsetY: 369,
                width: 64,
                height: 32,
                canWalk: 0		// 0 == yes
            },
            {
                offsetX: 1310,
                offsetY: 369,
                width: 64,
                height: 32,
                canWalk: 0		// 0 == yes
            },
            {
                offsetX: 1376,
                offsetY: 369,
                width: 64,
                height: 32,
                canWalk: 0		// 0 == yes
            },
            {
                offsetX: 1442,
                offsetY: 369,
                width: 64,
                height: 32,
                canWalk: 0		// 0 == yes
            },
            {
                offsetX: 1508,
                offsetY: 369,
                width: 64,
                height: 32,
                canWalk: 0		// 0 == yes
            },
            // last line
            {
                offsetX: 56,
                offsetY: 403,
                width: 64,
                height: 32,
                canWalk: 0		// 0 == yes
            },
            {
                offsetX: 122,
                offsetY: 403,
                width: 64,
                height: 32,
                canWalk: 0		// 0 == yes
            },
            {
                offsetX: 188,
                offsetY: 403,
                width: 64,
                height: 32,
                canWalk: 0		// 0 == yes
            },
            {
                offsetX: 254,
                offsetY: 403,
                width: 64,
                height: 32,
                canWalk: 0		// 0 == yes
            },
            {
                offsetX: 320,
                offsetY: 403,
                width: 64,
                height: 32,
                canWalk: 0		// 0 = false
            },
            {
                offsetX: 386,
                offsetY: 403,
                width: 64,
                height: 32,
                canWalk: 0		// 0 == yes
            },
            {
                offsetX: 452,
                offsetY: 403,
                width: 64,
                height: 32,
                canWalk: 0		// 0 == yes
            },
            {
                offsetX: 518,
                offsetY: 403,
                width: 64,
                height: 32,
                canWalk: 0		// 0 == yes
            },
            {
                offsetX: 584,
                offsetY: 403,
                width: 64,
                height: 32,
                canWalk: 0		// 0 == yes
            },
            {
                offsetX: 650,
                offsetY: 403,
                width: 64,
                height: 32,
                canWalk: 0		// 0 == yes
            },
            {
                offsetX: 716,
                offsetY: 403,
                width: 64,
                height: 32,
                canWalk: 0		// 0 == yes
            }
        ]
    },
	numCols = (map.width / map.tileWidth) | 0;

	map.triggers[14 * numCols + 3] = {
		type: 'wave',
		size: 1,
		spriteId: 'enemy1',
		afterDestroy: 'reward',
		afterDestroyData: 'apple',
		spriteOptions: {
			x: 65,
			y: 480,
            data: {
                damage: 2,
                health: 1,
                speed: 1,
                direction: 'Right'
			}
		}
	};

	map.triggers[14 * numCols + 6] = {
		type: 'wave',
		size: 2,
		spriteId: 'enemy1',
		afterDestroy: 'reward',
		afterDestroyData: 'apple',
		spriteOptions: {
			x: 704,
			y: 319,
            data: {
                health: 1,
                speed: 1,
                direction: 'Left',
                damage: 2
            }
		},
		delay: 1300
	};

/*    map.triggers[14 * numCols + 7] = {
		type: 'wave',
		size: 1,
		spriteId: 'flying-enemy1',
		afterDestroy: 'reward',
		afterDestroyData: 'chrystal',
		spriteOptions: {
			x: 704,
			y: 419,
            data: {
                health: 1,
                speed: 1,
                direction: 'Left',
                damage: 2
            }
		},
		delay: 1800
	};*/

    // TODO: place switch at the correct location
    map.triggers[16 * numCols + 12] = {
        type: 'explosion',
        targetId: 1,
        spriteId: 'enemy_explosion',
		spriteOptions: {
			x: 866,
			y: 478
		},
        conditions: [
            {
                type: 'switch',
                id: 1,
                status: true
            }
        ]
    };

    map.triggers[16 * numCols + 13] = {
        type: 'explosion',
        targetId: 2,
        spriteId: 'enemy_explosion',
		spriteOptions: {
			x: 908,
			y: 478
		},
        conditions: [
            {
                type: 'switch',
                id: 1,
                status: true
            }
        ]
    };

    map.triggers[14 * numCols + 7] = {
		type: 'message',
		message: 'Welcome to Gods HTML remake\nYou need to get the knife first'
    };

	map.triggers[14 * numCols + 8] = {
		type: 'wave',
		size: 1,
		spriteId: 'enemy1',
		afterDestroy: 'reward',
		afterDestroyData: 'apple',
		spriteOptions: {
			x: 129,
			y: 480,
            data: {
                health: 1,
                speed: 1,
                damage: 2,
                direction: 'Right'
            }
		},
		delay: 1300
	};

	map.triggers[14 * numCols + 9] = {
		type: 'cp'
    };

	map.mapItemBlocks[0] = {
		displayed: false,
		items: [{
            type: 'Gem',
            trigger: false,
            spriteOptions: {
                x: 80,
				y: 300
            }
        },
		{
			type: 'GodsSprite',
			trigger: false,
			spriteOptions: {
				master: true,
				x: 125,
				y: 448
			}
		},
        {
            type: 'SmallItem',
            trigger: false,
            spriteOptions: {
                x: 576,
                y: 460,
                data: {
                    itemType: 'knife',
                    fall: true
                }
            }
        },
        {
            type: 'SmallItem',
            trigger: false,
            spriteOptions: {
                x: 356,
                y: 460,
                data: {
                    itemType: 'help',
                    message: 'Get the knife',
                    fall: true
                }
            }
        },
        {
            type: 'SpearWood',
            trigger: false,
            itemId: 1,
            spriteOptions: {
                x: 886,
                y: 512, /* 496 */
                data: {
                    damage: 50
                }
            }
        },
        {
            type: 'SpearWood',
            trigger: false,
            itemId: 2,
            spriteOptions: {
                x: 928,
                y: 512,
                data: {
                    damage: 50
                }
            }
        },
		{
            type: 'Switch',
            trigger: false,
            spriteOptions: {
                objectId: 1,
                x: 832,
                y: 416,
				data: {

				}
            }
        }]
	};

    export default map;