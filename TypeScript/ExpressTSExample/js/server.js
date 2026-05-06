"use strict";
var __importDefault = (this && this.__importDefault) || function (mod) {
    return (mod && mod.__esModule) ? mod : { "default": mod };
};
Object.defineProperty(exports, "__esModule", { value: true });
const express_1 = __importDefault(require("express"));
const express_session_1 = __importDefault(require("express-session"));
const path_1 = __importDefault(require("path"));
const app = (0, express_1.default)();
// parameters
const DEPLOY = true;
let hostname = "127.0.0.1";
let port = 5500;
// expose on public IP (don't forget to open the port)
if (DEPLOY) {
    port = 13000;
    // get public IP address
    hostname = '0.0.0.0';
    const WhatsMyIpAddress = require('../js/WhatsMyIpAddress');
    WhatsMyIpAddress((data, err) => {
        if (err == null && data != null) {
            hostname = data;
            console.log(`Public IP address: http://${hostname}:${port}`);
        }
    });
}
////////////////////////////////// MIDDLEWARES ////////////////////////////////
// parse application/x-www-form-urlencoded
app.use(express_1.default.urlencoded({ extended: false }));
// parse application/json
app.use(express_1.default.json());
app.use((0, express_session_1.default)({
    secret: 'keyboard cat',
    resave: false,
    saveUninitialized: true,
    cookie: { secure: false } /* http */
}));
//////////////////////////////////// ROUTES ///////////////////////////////////
app.get('/', (request, response) => {
    const GetCurrentDateTime = require('../js/datetime');
    console.log(`${GetCurrentDateTime()}: Someone connected on ${request.url}`);
    if (request.session.error) {
        response.locals.error = request.session.error;
        request.session.error = undefined;
    }
    response.sendFile(path_1.default.join(__dirname, '../index.html'));
});
app.post('/', (request, response) => {
    const word = request.body?.word1;
    console.log(`New word: ${word} from ${request.socket.remoteAddress}`);
    if (word === undefined || word === '') {
        request.session.error = "There is an error";
        response.sendFile(path_1.default.join(__dirname, '../index.html'));
        return;
    }
    response.sendFile(path_1.default.join(__dirname, '../index.html'));
});
app.listen(port, hostname, () => {
    console.log(`Server running at http://${hostname}:${port}/`);
});
//# sourceMappingURL=server.js.map