import { Injectable } from '@angular/core';
import { environment } from '../../../environments/environment';
import { HttpClient, HttpParams, HttpHeaders } from '@angular/common/http';
import { of } from 'rxjs';
import { map } from 'rxjs/operators';



const httpOptions = {
  headers: new HttpHeaders({ 'Content-Type': 'application/json' })
};
const HttpUploadOptions = {
  headers: new HttpHeaders({ "Content-Type": "multipart/form-data" })
}

@Injectable({
  providedIn: 'root'
})

export class AreaService {

  URL = environment.URL_API;
  estados:any[] = [];
  tipoOrdenTrabajo :any[] = [];
 

  constructor(private http:HttpClient) { }

  get_mostrar_area(idEstado:number){
    let parametros = new HttpParams();
    parametros = parametros.append('opcion', '1');
    parametros = parametros.append('filtro', String(idEstado));

    return this.http.get( this.URL + 'tblServicios' , {params: parametros});
  }

  get_verificar_area(nombreArea:string){
    let parametros = new HttpParams();
    parametros = parametros.append('opcion', '3');
    parametros = parametros.append('filtro', nombreArea);

    return this.http.get( this.URL + 'tblServicios' , {params: parametros}).toPromise();
  }

  set_save_area(objMantenimiento:any){
    return this.http.post(this.URL + 'tblServicios', JSON.stringify(objMantenimiento), httpOptions);
  }

  set_edit_area(objMantenimiento:any, id_area :number){
    return this.http.put(this.URL + 'tblServicios/' + id_area , JSON.stringify(objMantenimiento), httpOptions);
  }

  set_anular_area(id_area : number){ 
    let parametros = new HttpParams();
    parametros = parametros.append('opcion', '2');
    parametros = parametros.append('filtro',  String(id_area));

    return this.http.get( this.URL + 'tblServicios' , {params: parametros});
  }


}