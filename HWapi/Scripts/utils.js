
var LOG = function (_msg, _obj) {

    var log = console.log;

    if (_obj == null || _obj == undefined)
        return log(_msg);

    return log(_msg, _obj);
}