
import { Component, OnInit, ElementRef, ViewChild, AfterViewInit } from '@angular/core';
import { FormGroup, FormControl } from '@angular/forms';
import { AlertasService } from '../../../services/alertas/alertas.service';
import { RespuestaServer } from '../../../models/respuestaServer.models';
import { FuncionesglobalesService } from '../../../services/funciones/funcionesglobales.service';
import { NgxSpinnerService } from 'ngx-spinner';
import { LoginService } from '../../../services/login/login.service';
import { from, combineLatest } from 'rxjs';
import Swal from 'sweetalert2';
 
import { BsDatepickerConfig } from 'ngx-bootstrap/datepicker';
import { SolicitudResguardoService } from '../../../services/Procesos/solicitud-resguardo.service';
 

declare var $:any;

@Component({
  selector: 'app-bandeja-asignacion-masivo',
  templateUrl: './bandeja-asignacion-masivo.component.html',
  styleUrls: ['./bandeja-asignacion-masivo.component.css']
})

export class BandejaAsignacionMasivoComponent implements OnInit  {

  formParamsFiltro : FormGroup;
  formParamsDet : FormGroup;
  
  idUserGlobal :number = 0;
  servicios :any[]=[]; 
 
  estados :any[]=[];  
  
  jefeCuadrillas :any[]=[];  
   
  solicitudesCab :any[]=[];  
  solicitudesDet :any[]=[];  
  efectivoPoliciales :any[]=[];  
  listResguardoEventos :any[]=[];  

  flag_modoEdicion = false;
  flagModo_EdicionDet = false;
  flagCerrar = false;

  idSolicitudCab_global :number = 0;
  idEstado_global :number = 9;

  cantEfect:number = 0;

  filtrarMantenimiento = "";
  datepiekerConfig:Partial<BsDatepickerConfig>;
  listSolicitudesTemp:any[]=[];  

  
   
  constructor(private alertasService : AlertasService, private spinner: NgxSpinnerService, private loginService: LoginService,  private funcionGlobalServices : FuncionesglobalesService, private  solicitudResguardoService : SolicitudResguardoService ) {         
    this.idUserGlobal = this.loginService.get_idUsuario();
  }
 
 ngOnInit(): void {
  this.inicializarFormularioFiltro();
 
  this.inicializarFormularioDet()
  this.getCargarCombos();

 }

 
 inicializarFormularioFiltro(){ 
    this.formParamsFiltro= new FormGroup({ 
      servicio : new FormControl('0'),
      fecha_ini : new FormControl(new Date()),
      fecha_fin : new FormControl(new Date()),
      idEstado : new FormControl('10')
     }) 
 }

 

inicializarFormularioDet(){ 
  this.formParamsDet= new FormGroup({ 
      id_Solicitud_Det : new FormControl('0'),
      id_Solicitud_Cab : new FormControl('0'),
      id_UsuarioEfectivoPolicial : new FormControl('0'),      
      estado : new FormControl('1'),
      usuario_creacion : new FormControl('0'),
  }) 
}

 
 getCargarCombos(){ 
  this.spinner.show();
  combineLatest([this.solicitudResguardoService.get_servicio(this.idUserGlobal),  this.solicitudResguardoService.get_estados(), this.solicitudResguardoService.get_jefeCuadrillas() ])
  .subscribe( ([ _servicios,_estados , _jefeCuadrillas])=>{
      this.servicios = _servicios;
 
      this.estados = _estados.filter(est=> est.id_Estado == 3 ||   est.id_Estado == 10 ||   est.id_Estado == 16   );
 

      this.jefeCuadrillas = _jefeCuadrillas; 

      if (  this.servicios.length> 0  ) { 
        this.formParamsFiltro.patchValue({"servicio": String(_servicios[0].id_Servicios) });
      }

    this.spinner.hide(); 
  },(error)=>{
    this.spinner.hide(); 
    alert(error);
  })
 }

 mostrarInformacion(){
      if (this.formParamsFiltro.value.fecha_ini == '' || this.formParamsFiltro.value.fecha_ini == null) {
        this.alertasService.Swal_alert('error','Por favor seleccione la fecha inicial');
        return 
      }     
      if (this.formParamsFiltro.value.fecha_fin == '' || this.formParamsFiltro.value.fecha_fin == null) {
        this.alertasService.Swal_alert('error','Por favor seleccione la fecha final');
        return 
      }   

      const fechaIni = this.funcionGlobalServices.formatoFecha(this.formParamsFiltro.value.fecha_ini); 
      const fechaFin = this.funcionGlobalServices.formatoFecha(this.formParamsFiltro.value.fecha_fin); 

      this.spinner.show();
      this.solicitudesCab = [];
      this.solicitudResguardoService.get_mostrar_solicitudesCab_bandeja_masiva(this.formParamsFiltro.value ,fechaIni,  fechaFin,  this.idUserGlobal)
          .subscribe((res:RespuestaServer)=>{            
              this.spinner.hide();   
              if (res.ok==true) {     
                  if(res.data.length > 0){ 
                    this.solicitudesCab = res.data;
                  }               
                }else{
                this.alertasService.Swal_alert('error', JSON.stringify(res.data));
                alert(JSON.stringify(res.data));
              }
      })
 }    
 
 cerrarModal(){
  setTimeout(()=>{ // 
        $('#modal_mantenimiento').modal('hide');  
        this.mostrarInformacion();
  },0); 
}
  
  

  efectivosPoliciales(objBD :any){

    Swal.fire({  icon: 'info', allowOutsideClick: false, allowEscapeKey: false, text: 'Espere por favor'  })
    Swal.showLoading();
    this.solicitudResguardoService.get_efectivosPoliciales( objBD.id_Solicitud_Cab, this.idUserGlobal ).subscribe((res:RespuestaServer)=>{
      Swal.close();   
      
      console.log(res.data);

      if (res.ok ==true) {         
        this.efectivoPoliciales = res.data;
        setTimeout(()=>{ // 
          $('#modal_efectivos').modal('show');
        },0); 
      }else{
        this.alertasService.Swal_alert('error', JSON.stringify(res.data));
        alert(JSON.stringify(res.data));
      }
    })
  }
 
  cerrarModalEfectivos(){
    setTimeout(()=>{ // 
      $('#modal_efectivos').modal('hide');  
    },0); 
  }
  
  descargarGrilla(){

    if (this.formParamsFiltro.value.fecha_ini == '' || this.formParamsFiltro.value.fecha_ini == null) {
      this.alertasService.Swal_alert('error','Por favor seleccione la fecha inicial');
      return 
    }     
    if (this.formParamsFiltro.value.fecha_fin == '' || this.formParamsFiltro.value.fecha_fin == null) {
      this.alertasService.Swal_alert('error','Por favor seleccione la fecha final');
      return 
    }   

    const fechaIni = this.funcionGlobalServices.formatoFecha(this.formParamsFiltro.value.fecha_ini); 
    const fechaFin = this.funcionGlobalServices.formatoFecha(this.formParamsFiltro.value.fecha_fin); 
  
    this.spinner.show();
    this.solicitudResguardoService.get_descargar_solicitudesCab(this.formParamsFiltro.value ,fechaIni,  fechaFin,  this.idUserGlobal)
        .subscribe((res:RespuestaServer)=>{            
            this.spinner.hide();
            console.log(res.data);
            if (res.ok==true) {        
              window.open(String(res.data),'_blank');
            }else{
              this.alertasService.Swal_alert('error', JSON.stringify(res.data));
              alert(JSON.stringify(res.data));
            }
    })
  }  

  resguardoEventos(){    
    Swal.fire({  icon: 'info', allowOutsideClick: false, allowEscapeKey: false, text: 'Espere por favor'  })
    Swal.showLoading();
    this.solicitudResguardoService.get_resguardoEventos_masivo(this.idUserGlobal ).subscribe((res:RespuestaServer)=>{
      Swal.close();             
      if (res.ok ==true) {         
        this.listResguardoEventos = res.data;
      }else{
        this.alertasService.Swal_alert('error', JSON.stringify(res.data));
        alert(JSON.stringify(res.data));
      }
    })
  }


  verificarEfectivoCargado(idUsuarioEfectivoPolicial: number){  
    var flagRepetida=false;
    for (const obj of this.solicitudesDet) {
      if (  obj.id_UsuarioEfectivoPolicial == idUsuarioEfectivoPolicial ) {
           flagRepetida = true;
           break;
      }
    }
    return flagRepetida;
  }

  blank_Detalle(){
    this.flagModo_EdicionDet= false;    
    this.inicializarFormularioDet();
   }


  getLiquidacionDet(){
    let codigos = this.funcionGlobalServices.obtenerTodos_IdPrincipal(this.listSolicitudesTemp,'id_Solicitud_Cab');

    this.solicitudResguardoService.get_solicitudDet_masivo( codigos.join(), this.idUserGlobal  ).subscribe((res:RespuestaServer)=>{
     if (res.ok) {       
       this.solicitudesDet = res.data; 

      //  ---verificando --
       if (this.solicitudesDet.length == this.cantEfect  ) {
        this.flagCerrar = true;
       }else{
           //  ---verificando --
           if (this.solicitudesDet.length > this.cantEfect  ) {
            this.flagCerrar = true;
           }else{
             this.flagCerrar = false;
           }
       }


       this.blank_Detalle();
     }else{
       this.alertasService.Swal_alert('error', JSON.stringify(res.data));
       alert(JSON.stringify(res.data));
       this.blank_Detalle();
     }    
    })        
  }

  guardarDet(){ 

    if ( this.listSolicitudesTemp.length == 0 ) {
      this.alertasService.Swal_alert('error', 'No hay solicitudes para asignar, verifique ..');
       return;
    }


    if (this.formParamsDet.value.id_UsuarioEfectivoPolicial == '' || this.formParamsDet.value.id_UsuarioEfectivoPolicial == 0 || this.formParamsDet.value.id_UsuarioEfectivoPolicial == null)  {
      this.alertasService.Swal_alert('error', 'Seleccione un Efectivo policial por favor.');
      return 
    } 

   let codigos = this.funcionGlobalServices.obtenerTodos_IdPrincipal(this.listSolicitudesTemp,'id_Solicitud_Cab');
    Swal.fire({
      icon: 'info',allowOutsideClick: false, allowEscapeKey: false,  text: 'Espere por favor'
    })
    Swal.showLoading();

    if (this.verificarEfectivoCargado( this.formParamsDet.value.id_UsuarioEfectivoPolicial ) ==true) {
      this.alertasService.Swal_alert('error', 'El Efectivo ya se cargo, verifique ..');
      return;
    }

    this.solicitudResguardoService.set_grabarSolicitudDetalle_masivo(this.formParamsDet.value.id_UsuarioEfectivoPolicial, codigos.join(), this.idUserGlobal  ).subscribe((res:RespuestaServer)=>{  
      Swal.close();
      if (res.ok) {  
          this.resguardoEventos();
          this.getLiquidacionDet();  
      }else{
        this.alertasService.Swal_alert('error', JSON.stringify(res.data));
        alert(JSON.stringify(res.data));
      }    
    })
  }

  
  eliminarSolicitudDetalle(item:any){ 
    let codigos = this.funcionGlobalServices.obtenerTodos_IdPrincipal(this.listSolicitudesTemp,'id_Solicitud_Cab'); 

    Swal.fire({
      icon: 'info', allowOutsideClick: false, allowEscapeKey: false, text: 'Espere por favor'
    })
    Swal.showLoading();
    this.solicitudResguardoService.set_eliminarSolicitudDetalle_masivo(codigos.join(), item.id_UsuarioEfectivoPolicial, this.idUserGlobal).subscribe((res:RespuestaServer)=>{
      Swal.close();
      if (res.ok) { 
          var index = this.solicitudesDet.indexOf( item );
          this.solicitudesDet.splice( index, 1 );
      }else{
        this.alertasService.Swal_alert('error', JSON.stringify(res.data));
        alert(JSON.stringify(res.data));
      }
    })
  }


  eliminarCheckMarcado(item:any){    
    var index = this.listSolicitudesTemp.indexOf( item );
    this.listSolicitudesTemp.splice( index, 1 );

   if ( this.listSolicitudesTemp.length == 0 ) {
     this.cerrarModal();
     return;
   }

    this.getLiquidacionDet();  

  }

  validacionCheckMarcado(){    
    let CheckMarcado = false;
    CheckMarcado = this.funcionGlobalServices.verificarCheck_marcado(this.solicitudesCab);
 
    if (CheckMarcado ==false) {
      this.alertasService.Swal_alert('error','Por favor debe marcar un elemento de la Tabla');
      return false;
    }else{
      return true;
    }
  }

  obtnerCheckMarcado_opcion(){
    let listRegistros =[];
    listRegistros = this.solicitudesCab.filter(ot => ot.checkeado ).map((obj)=>{
        return {  id_Solicitud_Cab: obj.id_Solicitud_Cab, descripcionArea: obj.descripcionArea, fechaAtencion : obj.fechaAtencion, cantidadEfectivos : obj.cantidadEfectivos } ;
    });
    return listRegistros;
  }


 


  asignarMasivo( ){

    if (this.validacionCheckMarcado()==false){
      return;
    }    

    this.listSolicitudesTemp = [];
    this.listSolicitudesTemp = this.obtnerCheckMarcado_opcion(); 
  
    if ( this.listSolicitudesTemp.length ==0 ) {
      this.alertasService.Swal_alert('error','Por favor verifique o complete el proceso');
      return;
    }

    this.cantEfect = 0;
    for (let index = 0; index < this.listSolicitudesTemp.length; index++) {
      if ( index == 0) {
        this.cantEfect = this.listSolicitudesTemp[index].cantidadEfectivos;
      }else{
          if ( this.cantEfect != this.listSolicitudesTemp[index].cantidadEfectivos ) {
            this.alertasService.Swal_alert('error', 'La cantidad de Efectivos de la solicitud no coinciden ,  \n Por favor seleccionar solicitudes  con cantidad de efectivos similares.');
            return;
          }
      }      
    }

    this.resguardoEventos();
    this.getLiquidacionDet();

    setTimeout(()=>{ // 
      $('#modal_mantenimiento').modal('show');
     },0); 

  } 

  cerrarAsignacion(){

    let codigos = this.funcionGlobalServices.obtenerTodos_IdPrincipal(this.listSolicitudesTemp,'id_Solicitud_Cab'); 

    this.alertasService.Swal_Question('Sistemas', 'Esta seguro de cerrar la asignacion ?')
    .then((result)=>{
      if(result.value){
 
        Swal.fire({  icon: 'info', allowOutsideClick: false, allowEscapeKey: false, text: 'Espere por favor'  })
        Swal.showLoading();
        this.solicitudResguardoService.set_cerrarAsignacion_masivo( codigos.join() , this.idUserGlobal ).subscribe((res:RespuestaServer)=>{
          Swal.close();        
          if (res.ok ==true) {             
            this.mostrarInformacion();
            this.alertasService.Swal_Success('Proceso realizado correctamente..')  
            this.cerrarModal(); 
          }else{
            this.alertasService.Swal_alert('error', JSON.stringify(res.data));
            alert(JSON.stringify(res.data));
          }
        })
         
      }
    }) 

  }


  anular(objBD:any){ 
    // if (objBD.idEstado === 10 || objBD.idEstado =='10' || objBD.idEstado ===11 || objBD.idEstado =='11') {      
    //   return;      
    // }

 
    this.alertasService.Swal_Question('Sistemas', 'Esta seguro de anular ?')
    .then((result)=>{
      if(result.value){
 
        Swal.fire({  icon: 'info', allowOutsideClick: false, allowEscapeKey: false, text: 'Espere por favor'  })
        Swal.showLoading();
        this.solicitudResguardoService.set_anularBandejaAtencion(objBD.id_Solicitud_Cab, this.idUserGlobal ).subscribe((res:RespuestaServer)=>{
          Swal.close();
  
          if (res.ok ==true) {             
            // for (const sol of this.solicitudesCab) {
            //   if (sol.id_Solicitud_Cab == objBD.id_Solicitud_Cab ) {               
            //     sol.idEstado = 11;
            //     sol.descripcion_estado =  "ANULADO" ;
            //     break;
            //   }
            // }
            this.mostrarInformacion();
            this.alertasService.Swal_Success('Se anulo correctamente..')  
 
          }else{
            this.alertasService.Swal_alert('error', JSON.stringify(res.data));
            alert(JSON.stringify(res.data));
          }
        })
         
      }
    }) 
 
  }
  
 

}

