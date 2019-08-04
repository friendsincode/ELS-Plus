module.exports = {
  outputDir: __dirname + "/NUI/app",
  publicPath: "./",
  filenameHashing: false,
  configureWebpack: {
    optimization: {
      splitChunks: {
        name: "els.plus"
      }
    }
  }
};
