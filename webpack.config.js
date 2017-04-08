var path = require('path'),
    HtmlWebpackPlugin = require('html-webpack-plugin'),
    WebpackNotifierPlugin = require('webpack-notifier');

module.exports = {
    entry: [
        'webpack-dev-server/client?http://127.0.0.1:8888',
        './gods/js/gods.js'
    ],
    output: {
        path: __dirname,
        filename: "bundle.js",
        pathinfo: true
    },
    devtool: 'eval',
    module: {
        rules: [
            { test: /\.js$/, loader: 'babel-loader?presets[]=es2015', exclude: /node_modules|athenajs\.js/ }
        ]
    },
    devServer: {
        host: '127.0.0.1',
        port: '8888',
        // inline hot-reload
        inline: true
    },
    resolve: {
        modules: [
            path.resolve('./gods/js'),
            'node_modules'
        ]

    },
    plugins: [
        new HtmlWebpackPlugin({
            title: 'Gods - AthenaJS',
            template: 'index-wp.ejs',
            inject: 'body'
        }),
        new WebpackNotifierPlugin({ alwaysNotify: true })
    ]
};