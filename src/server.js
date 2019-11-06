const http = require('http');
const fs = require('fs');
const path = require('path');
const info = require('./info.js');//mime type
const port=3000;

http.createServer(function (request, response) {
    console.log('request ', request.url);

    var filePath = '.' + request.url;
	var extname = String(path.extname(filePath)).toLowerCase();
	console.log(extname);
	if (!extname && fs.existsSync(filePath+'/index.html')) { //if directory
		filePath += '/index.html';
		extname = ".html";
    }
    var contentType = info.mime[extname] || 'application/octet-stream';
    filePath=decodeURI(filePath);
	fs.readFile(filePath, function(error, content) {
		//https://nodejs.org/api/errors.html#errors_common_system_errors
		if (error) {
			let errCode=500;//default
			let errorMsg;
			switch(error.code){
				case 'EPERM'://401?
					errCode=401;
					errorMsg="401 Unauthorized";
					break;
				case 'EACCES'://403
					errCode=403;
					errorMsg="403 Permission denied";
					break;
				case 'ENOENT': //404
					errCode=404;
					errorMsg="404 Not found";
					break;
				case 'EISDIR': //oops it's dir
					errCode=403;
					errorMsg="You're trying to see the directory";
					break;
				case 'ECONNRESET'://connection reset
					errorMsg="Connection reset";
					break;
				case 'ETIMEDOUT'://timeout
					errCode=408;//not sure ;)
					errorMsg="Connection timeout. Check if socket is properly closed.";
					break;
				default:
					errorMsg="Unknown error. Could be wrong code?";
					break;
			}
			response.writeHead(errCode);
            response.end(errorMsg, 'utf-8');
		}
        else {
            response.writeHead(200, { 'Content-Type': contentType });
            response.end(content, 'utf-8');
        }
    });

}).listen(port);
console.log('Server running at http://127.0.0.1:'+port+'/');
