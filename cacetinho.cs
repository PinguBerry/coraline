using System.Collections;
using System.Collections.Generic;
using System.IO.Ports;
using UnityEngine;

public class cacetinho: MonoBehaviour
{
    public GameObject fantasmas;
    public GameObject fantasmashappy;
    public GameObject cenariofora1;
    
    public GameObject cenariofora2;
    public GameObject salabellman;
    public GameObject salabellmannormal;
    public GameObject portal;
    public GameObject chuvinhaa;

    public GameObject coralinebase;   
    public GameObject coralinebasebotao;
    public GameObject branco;
    public GameObject olhos;
    public GameObject botao;

    public GameObject audio1;
    public GameObject audio2;
    public GameObject audio3;
    public GameObject audio4;
    public GameObject audio5;
    public GameObject audio6;
    public GameObject audio7;
    
    SerialPort coraline = new SerialPort("COM13", 9600);

    SerialPort portall = new SerialPort("COM4", 9600);

    string men = "";
    string message = "";

    float x = 0.00f;
    float y = 0.00f;
    float coordx = 0.00f;
    float coordy = 0.00f;

    bool podex = false;
    bool podey = false;
    bool okx = false;
    bool oky = false;


    bool ok2 = false;
    bool ok3 = false;
    bool ok4 = false;
    bool ok5 = false;
    bool ok6 = false;
    bool ok7 = false;

    bool o = false;
    bool o1 = false;


    bool pode = false;

    bool inv = false;

    int ato = 0;

    float tempo = 0f;
    float tempo2= 0f;
    float duracao = 3f;

    void Men()
    {

        if (portall.IsOpen)
        {
            Debug.Log("Portal is open!");
        }
    }


    void Men2(){

        if (coraline.IsOpen)
        {
            Debug.Log("Coraline is open!");
        }

    } 

    public void Aparece(){
                
        if (tempo <= 0f)
        {
            ato++;
            tempo = duracao;
            UnityEngine.Debug.Log(ato);
        }
    }

      
    void OlhoNormal(){
        Debug.Log("entrei aqui");

        try{

            if (coraline.BytesToRead > 0){

                //men = coraline.ReadLine();

                char men = (char) coraline.ReadChar();

                if(men == 'x') okx = true;
                if(men == 'y') oky = true;

                if(oky){

                    if (men == '#'){
                        Debug.Log("RECEBI Y " + message);
                        oky = false;
                        y = float.Parse(message);

                        message = "";
                        men = ' ';

                        podey = true;

                    } else {
                        if (men != 'y') message += men;
                    }

                } else if (okx){

                    if (men == '#'){//add if aqui --> x ou y!!!!
                        Debug.Log("RECEBI X " + message);
                        okx = false;
                        x = float.Parse(message);

                        message = "";
                        men = ' ';

                        podex = true;

                    } else {
                        if (men != 'x') message += men;
                    }

                }


            } 
            
        } catch {
            UnityEngine.Debug.Log("<color=#FFFF00> ERRO </color>");
        }

        if(podey){

            if(y >= 0 && y <= 30){
                
                coordy = (2*y) - 60;

            } else if(x <= 70){

                coordy = ((60*y) - 1800) / 35;
            }

            olhos.GetComponent<RectTransform>().localPosition = new Vector3(0, coordy, 0);
            podey = false;

        } else if(podex) {

            if(x >= 50 && x <= 90){
                
                coordx = ((45*x) - 2250) / (40);

            } else if (x >= 10) {

                coordx = ((-45*x) + 2250) / (-40);
            }

            olhos.GetComponent<RectTransform>().localPosition = new Vector3(coordx, 0, 0);
            podex = false;

        }
    }

    void Sala(){
        try{

            if (coraline.BytesToRead > 0){

                //men = coraline.ReadLine();

                char men = (char) coraline.ReadChar();

                if(men == 'x') okx = true;
                if(men == 'y') oky = true;

                if(oky){

                    if (men == '#'){
                        Debug.Log("RECEBI Y " + message);
                        oky = false;
                        y = float.Parse(message);

                        message = "";
                        men = ' ';

                        podey = true;

                    } else {
                        if (men != 'y') message += men;
                    }

                } else if (okx){

                    if (men == '#'){//add if aqui --> x ou y!!!!
                        Debug.Log("RECEBI X " + message);
                        okx = false;
                        x = float.Parse(message);

                        message = "";
                        men = ' ';

                        podex = true;

                    } else {
                        if (men != 'x') message += men;
                    }

                }


            } 
            
        } catch {
            UnityEngine.Debug.Log("<color=#FFFF00> ERRO </color>");
        }

        if(podey){

            if(y >= 0 && y <= 30){
                
                coordy = ((349*y) - 10470) / 30;

            } else if(x <= 70){

                coordy = ((-349*y) + 10470) /  (-30);
            }

            salabellman.GetComponent<RectTransform>().localPosition = new Vector3(0, coordy, 0);
            podey = false;

        } else if(podex) {

            if(x >= 50 && x <= 90){
                
                coordx = ((615*x) - 30750) / (40);

            } else if (x >= 10) {
                
                
                coordx = ((-615*x) + 30750) / (-40);
                
            }

            salabellman.GetComponent<RectTransform>().localPosition = new Vector3(coordx, 0, 0);
            podex = false;

        }
    }

    void Start()
    {
        salabellman.GetComponent<RectTransform>().localPosition = new Vector3(x, y, 0);

        salabellman.SetActive(false);
        salabellmannormal.SetActive(false);
        chuvinhaa.SetActive(false);
        fantasmas.SetActive(false);
        olhos.SetActive(false);
        branco.SetActive(false);
        coralinebase.SetActive(false);

        cenariofora1.SetActive(true);
        coraline.WriteTimeout = 300;
        coraline.ReadTimeout = 120000;

        audio1.GetComponent<AudioSource>().Play();
        coraline.Open();
        portall.Open();

        Men();
        Men2();
    }

    bool okc = false;
    void Update()
    {
        
        if (tempo > 0f)
        {
            tempo -= Time.deltaTime;
        }

        


            if (ato == 1)//chuva
            {
                cenariofora1.SetActive(true);
                chuvinhaa.SetActive(true);

                audio1.GetComponent<AudioSource>().Stop();
                

                if(okc == false){
                    chuvinhaa.GetComponent<ParticleSystem>().Play();
                    okc = true;
                }
                
                if(ok2 == false){
                    audio2.GetComponent<AudioSource>().Play();
                    ok2 = true;
                }

                try{
                    coraline.Write("1");
                    coraline.Write("1");
                    coraline.Write("1");
                } catch {
                    Debug.Log("erro");
                }
                //audio
            }
            else if (ato == 2)//estender a mão
            {
                //audio olá eu sou coraline jones blablabla
            
                if(ok3 == false){
                    audio3.GetComponent<AudioSource>().Play();
                    ok3 = true;
                }

                chuvinhaa.GetComponent<ParticleSystem>().Play();
                
                try{
                    coraline.Write("2");
                    coraline.Write("2");
                    coraline.Write("2");
                } catch {
                    Debug.Log("erro");
                }

            }
            else if (ato == 3)//duvida
            {   //eu fiquei sabendo que tem algo muito legal na sua casa (atras de uma portinha)
                //vai lá descobrir
                audio3.GetComponent<AudioSource>().Stop();
                chuvinhaa.GetComponent<ParticleSystem>().Play();

                try{
                    coraline.Write("3");
                    coraline.Write("3");
                    coraline.Write("3");
                } catch {
                    Debug.Log("erro");
                }

            }
            else if (ato == 4)//portal //PLANO B
            {
                chuvinhaa.SetActive(false);
                chuvinhaa.GetComponent<ParticleSystem>().Stop();
                portal.SetActive(true);

                try{
                    coraline.Write("4");
                    coraline.Write("4");
                    coraline.Write("4");

                    portall.Write("p");
                    portall.Write("p");
                    portall.Write("p");//plano B

                } catch {
                    Debug.Log("erro");
                }
                

            } else if(ato == 5){//zoiando

                portal.SetActive(false);
                salabellmannormal.SetActive(true);

                //Sala();
            }
            else if (ato == 6)//presente
            {
                /*try{
                    
                    if(coraline.BytesToRead > 0){
                        char men = (char) coraline.ReadChar();

                        if(men == '8'){//na vdd é 8

                            //salabellman.SetActive(false);
                            //olhos.SetActive(true);
                            //coralinebase.SetActive(true);
                            //branco.SetActive(true);

                            salabellman.SetActive(false);
                            audio4.GetComponent<AudioSource>().Stop();
                            
                            coralinebasebotao.SetActive(true);
                            botao.SetActive(true);

                            if(ok4 == false){
                                audio4.GetComponent<AudioSource>().Play();
                                ok4 = true;
                            }

                        }
                    }

                } catch {
                    Debug.Log("erro");
                }*/
                    



                //OlhoNormal();
                salabellmannormal.SetActive(false);
                coralinebasebotao.SetActive(true);
                botao.SetActive(true);

                if(ok4 == false){
                    audio4.GetComponent<AudioSource>().Play();
                    ok4 = true;
                }

                try{
                    coraline.Write("5");
                    coraline.Write("5");//naonaonaonao
                    coraline.Write("5");
                } catch {
                    Debug.Log("erro");
                }

                //OlhoNormal();

            } /*else if(ato == 7){//olhos botao

                audio4.GetComponent<AudioSource>().Stop();
                olhos.SetActive(false);
                branco.SetActive(false);
                coralinebase.SetActive(false);
                coralinebasebotao.SetActive(true);
                botao.SetActive(true);
            }*/
            else if (ato == 7)//fantasma
            {   
                coralinebasebotao.SetActive(false);
                botao.SetActive(false);
                salabellmannormal.SetActive(true);
                fantasmas.SetActive(true);

                if(ok6 == false){
                    audio6.GetComponent<AudioSource>().Play();
                    ok6 = true;
                }//nos ajude coraline

                //coraline.Write("8");
                //coraline.Write("8");
                //coraline.Write("8");

            }
            else if (ato == 8)//tchauzinho
            {   
                if(ok5 == false){
                    audio5.GetComponent<AudioSource>().Play();
                    ok5 = true;
                }//vms gnt nos ajude

                //coraline.Write("9");
                //coraline.Write("9");
                //coraline.Write("9");//

            }else if (ato == 9)//tchauzinho
            {   
                fantasmas.SetActive(false);
                fantasmashappy.SetActive(true);

                if(ok7 == false){
                    audio7.GetComponent<AudioSource>().Play();
                    ok7 = true;
                }
                    
                /*try{
                    
                    if(coraline.BytesToRead > 0){
                        char men = (char) coraline.ReadChar();

                        if(men == '9'){

                            fantasmas.SetActive(false);
                            fantasmashappy.SetActive(true);

                            if(ok7 == false){
                                audio7.GetComponent<AudioSource>().Play();
                                ok7 = true;
                            }

                            o1 = true;

                        }
                    }

                    
                } catch {
                    Debug.Log("erro");
                }*/


            } 
                
                
                

                /*fantasmas.SetActive(false);
                fantasmashappy.SetActive(true);

                if(ok7 == false){
                    audio7.GetComponent<AudioSource>().Play();
                    ok7 = true;
                }//obrigada e tchauzinho 

                try{
                    coraline.Write("7");
                    coraline.Write("7");
                    coraline.Write("7");
                } catch {
                    Debug.Log("erro");
                }*

            }*/

    }
}