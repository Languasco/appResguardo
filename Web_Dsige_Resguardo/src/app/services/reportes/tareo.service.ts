
 
import { Injectable } from '@angular/core';
import { environment } from '../../../environments/environment';
import { HttpClient, HttpParams, HttpHeaders } from '@angular/common/http';
import { map } from 'rxjs/operators';
import { of } from 'rxjs';

const httpOptions = {
  headers: new HttpHeaders({ 'Content-Type': 'application/json' })
};
const HttpUploadOptions = {
  headers: new HttpHeaders({ "Content-Type": "multipart/form-data" })
}
@Injectable({
  providedIn: 'root'
})
export class TareoService {


  URL = environment.URL_API;
  estados :any [] = [];

  constructor(private http:HttpClient) { }

  get_mostrarTareoCab(idServicio:number , fecha_ini:string , fecha_fin:string){ 
    let parametros = new HttpParams();
    parametros = parametros.append('opcion', '8');
    parametros = parametros.append('filtro', idServicio + '|' +  fecha_ini + '|' +  fecha_fin );
    return this.http.get( this.URL + 'Reportes' , {params: parametros});
  }

  get_descargarTareoCab(idServicio:number , fecha_ini:string , fecha_fin:string, idUser:number){ 
    let parametros = new HttpParams();
    parametros = parametros.append('opcion', '9');
    parametros = parametros.append('filtro', idServicio + '|' +  fecha_ini + '|' +  fecha_fin + '|' +  idUser  );

    console.log(parametros)
    
    return this.http.get( this.URL + 'Reportes' , {params: parametros});
  }

}
