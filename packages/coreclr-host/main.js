const curPlatform = process.platform;

if (
  curPlatform !== 'darwin' &&
  curPlatform !== 'linux' &&
  curPlatform !== 'win32'
){
  throw `Unsupported platform ${curPlatform}.`;
}

module.exports = require(`./${curPlatform}/al90-coreclr-addon.node`);
