const path = require('path');

module.exports = {
    entry: [
        './Client/js/main.js',
    ]
    ,
    output: {
        path: path.resolve(__dirname, 'wwwroot/js'),
        filename: '[name].js'
    }
    ,
    module: {
        rules: [
            {
                test: /\.css$/,
                loader: ["style-loader", "css-loader"]
            }
        ]
    }
};

