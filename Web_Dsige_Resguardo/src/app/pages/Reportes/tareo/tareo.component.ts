
import { Component, OnInit, ElementRef, ViewChild, AfterViewInit } from '@angular/core';
import { FormGroup, FormControl } from '@angular/forms';
import { AlertasService } from '../../../services/alertas/alertas.service';
import { RespuestaServer } from '../../../models/respuestaServer.models';
import { FuncionesglobalesService } from '../../../services/funciones/funcionesglobales.service';
import { NgxSpinnerService } from 'ngx-spinner';
import { LoginService } from '../../../services/login/login.service';
import { from, combineLatest } from 'rxjs';
import Swal from 'sweetalert2';

import { UsuariosService } from '../../../services/Mantenimientos/usuarios.service';
import { TareoService } from '../../../services/reportes/tareo.service';

import { jsPDF } from "jspdf";


@Component({
  selector: 'app-tareo',
  templateUrl: './tareo.component.html',
  styleUrls: ['./tareo.component.css']
})
export class TareoComponent implements OnInit {

  formParamsFiltro : FormGroup;
  formParamsDatosG : FormGroup;  

  idUserGlobal :number = 0;
  tareoCab :any[]=[];  
  servicios :any[]=[];   
  filtrarMantenimiento = "";

  @ViewChild('htmlData', {static: false}) htmlData: ElementRef;
  
  constructor(private alertasService : AlertasService, private spinner: NgxSpinnerService, private loginService: LoginService , private funcionGlobalServices : FuncionesglobalesService, private usuariosService : UsuariosService, private tareoService :TareoService ) {         
    this.idUserGlobal = this.loginService.get_idUsuario();
   }

  ngOnInit(): void {
    this.inicializarFormularioFiltro()
    this.getCargarCombos();
  }


  inicializarFormularioFiltro(){ 
    this.formParamsFiltro= new FormGroup({
      Codservicio : new FormControl('0'),
      fecha_ini : new FormControl(new Date()),
      fecha_fin : new FormControl(new Date()), 
     }) 
 }
  
 getCargarCombos(){ 
  this.spinner.show();
  combineLatest([ this.usuariosService.get_area() ]).subscribe( ([ _servicios ])=>{
    this.servicios = _servicios;
    this.spinner.hide(); 
  },(error)=>{
    this.spinner.hide(); 
    alert(error);
  })
 }

 mostrarInformacion(){
    // if (this.formParamsFiltro.value.Codservicio == '' || this.formParamsFiltro.value.Codservicio == 0) {
    //   this.alertasService.Swal_alert('error','Por favor seleccione el servicio');
    //   return 
    // } 
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
    this.tareoService.get_mostrarTareoCab(this.formParamsFiltro.value.Codservicio ,fechaIni, fechaFin)
        .subscribe((res:RespuestaServer)=>{            
            this.spinner.hide();
            if (res.ok==true) {        
                this.tareoCab = res.data;  
             }else{
              this.alertasService.Swal_alert('error', JSON.stringify(res.data));
              alert(JSON.stringify(res.data));
            }
    })
 } 

 descargarGrilla(){

  if (this.formParamsFiltro.value.Codservicio == '' || this.formParamsFiltro.value.Codservicio == 0) {
    this.alertasService.Swal_alert('error','Por favor seleccione el servicio');
    return 
  } 
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
  this.tareoService.get_descargarTareoCab(this.formParamsFiltro.value.Codservicio ,fechaIni, fechaFin, this.idUserGlobal)
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
 generarPDF(){
  const doc = new jsPDF(); 
  let altura = 30;
  // doc.text("Hello world!", 10, 10);


  doc.setFontSize(15);
  doc.setFont("courier");
  doc.setTextColor("#17202A");

  doc.text('PARTE DIARIO EFECTIVO POLICIAL',60, 20, );
  doc.setTextColor("#808080");
  doc.setFontSize(10);
  altura = altura + 6;
  doc.text('AREA :',10, altura, );

  altura = altura + 6;
  doc.text('FECHA :',10, altura, );     doc.text(' TOTAL DE HORAS :', 100, altura, );

  altura = altura + 6;
  doc.text('HORA DE INICIO :',10, altura, );     doc.text(' HORA FIN :', 100, altura, );

  altura = altura + 6;
  doc.setTextColor("#000000"); //// ---negrita
  doc.text('NOMBRE DEL COORDINADOR :',10, altura, );    
  doc.setTextColor("#212F3D");
  altura = altura + 4;
  doc.text('Ing segovia :', 15, altura, ); 
  doc.line(10, altura + 1.5, 200, altura + 1.5) // horizontal line

  altura = altura + 8;
  doc.setTextColor("#000000"); //// ---negrita
  doc.text('NOMBRE Y APELLIDO DEL EFECTIVO POLICIAL :',10, altura, );   
  doc.setTextColor("#212F3D");
  altura = altura + 4;
  doc.text('Mario Calvo Seminario :',15, altura, ); 
  doc.line(10, altura + 1.5, 200, altura + 1.5) // horizontal line


  altura = altura + 8;
  doc.setTextColor("#000000"); //// ---negrita
  doc.text('JEFE DE CUADRILLA :',10, altura);  
  doc.setTextColor("#212F3D");
  altura = altura + 4;
  doc.text('Tco. Torres Lopez Jorge :',15, altura, ); 
  doc.line(10, altura + 1.5, 200, altura + 1.5) // horizontal line


  altura = altura + 8;
  doc.setTextColor("#000000"); //// ---negrita
  doc.text('LUGAR DE TRABAJO :',10, altura );  

  altura = altura + 5;
  doc.setTextColor("#212F3D");
  let splitTitle = doc.splitTextToSize(String('Adobe formulated PDF at around 1990s. It has two primary goals. The first goal was that users should be able to open the documents on any hardware or operating system. The second goal was that whenever a user opens a PDF document that must look the same.'), 190);

  doc.text(splitTitle,10, altura); 
  let _val = 0;

  if (splitTitle.length == 0) {
    _val = 10;
  } else {
    if (splitTitle.length == 1) {
        _val = 10;
    } else {
        _val = (6 * splitTitle.length);
    }
  }
  altura = (altura + _val);
 
  doc.setTextColor("#000000"); //// ---negrita
  doc.text('OBSERVACION :',10, altura );  

  altura = altura + 5;
  doc.setTextColor("#212F3D");
  splitTitle = doc.splitTextToSize(String('jsPDF cannot live without help from the community! If you think a feature is missing or you found a bug, please consider if you can spare one or two hours and'), 190);
  doc.text(splitTitle,10, altura); 

  doc.output('dataurlnewwindow');
 }


 

 

  

}
