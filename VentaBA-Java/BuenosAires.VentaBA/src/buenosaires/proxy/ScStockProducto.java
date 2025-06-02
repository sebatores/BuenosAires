/*
 * Click nbfs://nbhost/SystemFileSystem/Templates/Licenses/license-default.txt to change this license
 * Click nbfs://nbhost/SystemFileSystem/Templates/Classes/Class.java to edit this template
 */
package buenosaires.proxy;

import buenosaires.proxy2.IWsStockProducto;
import buenosaires.proxy2.WsStockProducto;
import java.util.ArrayList;
import org.json.simple.JSONObject;
import org.json.simple.JSONValue;
import org.json.simple.JSONArray;
/**
 *
 * @author Ruh
 */
public class ScStockProducto {
    
    public String accion = "";
    public String mensaje = "";
    public Boolean hayErrores = false;
    public String jsonStockProducto = "";
    public ArrayList<StockProducto> lista = new ArrayList<StockProducto>();
    
    public ScStockProducto(){
        accion = "";
        mensaje = "";
        hayErrores = false;
        jsonStockProducto = "";
        lista = null;     
    }
    public String getAccion(){
        return accion; 
        
    }
    public void setAccion(String accion){
        this.accion = accion;
    }
    public String getMensaje(){
        return mensaje;
    }
    public void setMensaje(String mensaje){
        this.mensaje = mensaje;
    }
    public Boolean getHayErrores(){
        return hayErrores;
    }
    public void setHayErrores(Boolean hayErrores){
       this.hayErrores = hayErrores; 
    }
    public String getJsonStockProducto(){
        return jsonStockProducto;
    } 
    public void setJsonStockProducto(String jsonStockProducto){
        this.jsonStockProducto = jsonStockProducto;
    }
    public ArrayList<StockProducto> getLista(){
        return lista; 
    }
    public void setLista(ArrayList<StockProducto> lista){
        this.lista = lista;
    }
    
    public void ejecutarPruebas() {
        WsStockProducto ws = new WsStockProducto();
        IWsStockProducto port = ws.getBasicHttpBindingIWsStockProducto();
        
        
        buenosaires.proxy2.Respuesta resp = port.obtenerEquiposEnBodega();
        String json1 = resp.getJsonStockProducto().getValue();
        Object jsonObject = JSONValue.parse(json1);
        
        JSONArray jsonArrayList = (JSONArray) jsonObject;
        ArrayList<Object> arrayList = new ArrayList<>();
        
        for (int i = 0; i < jsonArrayList.size(); i++){
            arrayList.add(jsonArrayList.get(i));
        }
        
        
    }
    
    public ArrayList<Object> ObtenerEquiposEnBodega() {
        WsStockProducto ws = new WsStockProducto();
        IWsStockProducto port = ws.getBasicHttpBindingIWsStockProducto();

        buenosaires.proxy2.Respuesta resp = port.obtenerEquiposEnBodega();

        String json1 = resp.getJsonStockProducto().getValue();
        System.out.print(json1);
        
        Object jsonObject = JSONValue.parse(json1);
        
        JSONArray jsonArrayList = (JSONArray) jsonObject;
        ArrayList<Object> arrayList = new ArrayList<>();
        System.out.println("Respuesta JSON recibida:");
        System.out.println(json1);
        for(int i = 0; i< jsonArrayList.size(); i++){
            arrayList.add(jsonArrayList.get(i));
        }
            System.out.println("Respuesta JSON recibida:");
            System.out.println(json1);
        return arrayList;

    }
}
