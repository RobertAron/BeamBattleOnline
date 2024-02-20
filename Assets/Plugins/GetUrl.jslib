mergeInto(LibraryManager.library, {
    GetURLFromPage: function () {
        try {
            var returnStr = window.top.location.href;
            var bufferSize = lengthBytesUTF8(returnStr) + 1;
            var buffer = _malloc(bufferSize);
            stringToUTF8(returnStr, buffer, bufferSize);
            return buffer;
        } catch(err){
            var returnStr = "";
            var bufferSize = lengthBytesUTF8(returnStr) + 1;
            var buffer = _malloc(bufferSize);
            stringToUTF8("", buffer, bufferSize);
        }
    }
});