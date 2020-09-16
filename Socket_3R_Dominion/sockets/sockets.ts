import { Socket  } from 'socket.io';
import socketIO from 'socket.io';

export const mensajes = (cliente: Socket, io: socketIO.Server ) =>{
  ///escuchar eventos
  cliente.on('Notificacion_web_OT',( payload : any, callback:any)=>{
    callback({ok:true, data:'Se recibio correctamente en el servidor socket'});
    /// emtir mensajes
    // io.emit('Alertas_web_OT', payload);
    cliente.broadcast.emit('Alertas_web_OT', payload);
  })

  cliente.on('Notificacion_movil_web_OT',( payload : any)=>{
    /// emtir 
    io.emit('Alertas_movil_web_OT', payload);
  })

  cliente.on('Notificacion_movil_OT',( payload : any)=>{
      /// emtir 
      io.emit('Alertas_movil_OT', payload);
  })
}



 



