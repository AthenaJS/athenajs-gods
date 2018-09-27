var path = require('path'),
    HtmlWebpackPlugin = require('html-webpack-plugin'),
    WebpackNotifierPlugin = require('webpack-notifier');

module.exports = {
    entry: [
        './gods/src/gods.ts'
    ],
    output: {
        path: __dirname,
        filename: "bundle.js",
        pathinfo: true
    },
    devtool: 'source-map',
    mode: 'development',
    module: {
        rules: [
            {
                test: /\.(js|map)$/,
                use: ["source-map-loader"],
                enforce: "pre",
                include: /athena/
            },
            {
                test: /\.(t|j)sx?$/,
                use: {
                    loader: 'awesome-typescript-loader'
                },
                exclude: /athena/
            }
        ]
    },
    devServer: {
        host: '0.0.0.0',
        port: '8888',
        // inline hot-reload
        inline: true
    },
    resolve: {
        extensions: ['.ts', '.js']
    },
    plugins: [
        new HtmlWebpackPlugin({
            title: 'Gods - AthenaJS',
            template: 'index-wp.ejs',
            inject: 'body'
        }),
        new WebpackNotifierPlugin({
            alwaysNotify: true,
            skipFirstNotification: true,
            title: 'AthenaJS-Gods',
            contentImage: path.join(__dirname, './gods/img/godsIntro.jpg')
        })
    ]
};