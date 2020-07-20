var _JS_UNETWebSockets_SocketCreate_Original = _JS_UNETWebSockets_SocketCreate;
_JS_UNETWebSockets_SocketCreate = function (hostId, urlPtr) {
  var url = window.top.location.href;
  var ishttps = url.includes('https');
  if(!ishttps) return _JS_UNETWebSockets_SocketCreate_Original(hostId, urlPtr);
  var url = Pointer_stringify(urlPtr).replace(/^ws:\/\//, "wss://");
  urlPtr = stackAlloc((url.length << 2) + 1);
  stringToUTF8(url, urlPtr, (url.length << 2) + 1);
  return _JS_UNETWebSockets_SocketCreate_Original(hostId, urlPtr);
};