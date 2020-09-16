import  express  from 'express';
import { SERVE_PORT } from '../global/environment';

///---socket no puede trabajar directamente con express lo tiene que hacer por medio de http
import socketIO from 'socket.io';
import http from 'http';

///----realizandolo en un archivo aparte ----
import  *  as socketCliente from '../sockets/sockets';
 
export default class Server {
 
    private static _instance: Server
    
    public app: express.Application ;
    public port: number;
    
    public io : socketIO.Server;
    private	httpServer: http.Server;
 

   private constructor(){
        this.app = express();
        this.port  = SERVE_PORT;
        this.httpServer =  new http.Server(this.app);
        this.io = socketIO( this.httpServer);

        this.escucharSocket();
    }  

    //pratron singleton
    public static get Instance(){
        return this._instance || (this._instance  = new this())
    }

    private escucharSocket(){
        console.log('escuchando conexion - socket');

        this.io.on('connection', cliente=>{       
            socketCliente.mensajes(cliente, this.io);        
        })


    }

    start( callback : Function){
       this.httpServer.listen(this.port, callback()) ;
    }

}