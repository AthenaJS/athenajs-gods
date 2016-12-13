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
    // devtool: 'cheap-module-eval-source-map',
    devtool: 'eval',
    module: {
        loaders: [
            { test: /\.js$/, loader: 'babel-loader?presets[]=es2015', exclude: /node_modules|athenajs\.js/ }            
        ]
    },
    devServer: {
        address: '127.0.0.1',
        port: '8888',
        // inline hot-reload
        inline: true
    },
    resolve: {
        modulesDirectories: ['node_modules'],
        root: [path.resolve('./gods/js')]
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