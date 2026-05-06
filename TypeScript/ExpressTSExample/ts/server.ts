import express, { Request, Response } from 'express';
import session from 'express-session';
import path from 'path';

declare module 'express-session' {
    interface SessionData {
        error?: string;
    }
}

const app = express();

// parameters
const DEPLOY: boolean = true;
let hostname: string = "127.0.0.1";
let port: number = 5500;

// expose on public IP (don't forget to open the port)
if (DEPLOY)
{
    port = 13000;

    // get public IP address
    hostname = '0.0.0.0';
    const WhatsMyIpAddress = require('../js/WhatsMyIpAddress');
    WhatsMyIpAddress((data: string | null, err: string | null) => {
        if (err == null && data != null)
        {
            hostname = data;
            console.log(`Public IP address: http://${hostname}:${port}`);
        }
    });
}

////////////////////////////////// MIDDLEWARES ////////////////////////////////


// parse application/x-www-form-urlencoded
app.use(express.urlencoded({ extended: false }));

// parse application/json
app.use(express.json());

app.use(session({
    secret: 'keyboard cat',
    resave: false,
    saveUninitialized: true,
    cookie: { secure: false } /* http */
}));


//////////////////////////////////// ROUTES ///////////////////////////////////


app.get('/', (request: Request, response: Response) => {

    const GetCurrentDateTime = require('../js/datetime');
    console.log(`${GetCurrentDateTime()}: Someone connected on ${request.url}`);

    if (request.session.error)
    {
        response.locals.error = request.session.error;
        request.session.error = undefined;
    }

    response.sendFile(path.join(__dirname, '../index.html'));
});

app.post('/', (request: Request, response: Response) => {

    const word = request.body?.word1;
    console.log(`New word: ${word} from ${request.socket.remoteAddress}`);
    if (word === undefined || word === '')
    {
        request.session.error = "There is an error";
        response.sendFile(path.join(__dirname, '../index.html'));
        return;
    }

    response.sendFile(path.join(__dirname, '../index.html'));
});

app.listen(port, hostname, () => {
    console.log(`Server running at http://${hostname}:${port}/`);
});
