#include <VarSpeedServo.h>
#include <AccelStepper.h>

#define STEP_PIN 5
#define DIR_PIN 4

/*#define STEP_PIN 3
#define DIR_PIN 4*/

//mudar para a maozinha 2
float dif2 = 3.70;
float dif1 = 7.45;

int angulo = 0; // declara origem do angulo como 0°


VarSpeedServo s11; // cria o objeto servo
VarSpeedServo s12;
VarSpeedServo s13;

AccelStepper stepper(AccelStepper::DRIVER, STEP_PIN, DIR_PIN);

void d1p(){
  s13.write(100, 200, true);
}

void d1b(){
  s13.write(40, 200, true);
}

void d2p(){
  s12.write(110, 200, true);
  delay(50);
  s12.write(155, 250, true);
}

void d2b(){
  s12.write(45, 200, true);
  delay(50);
  s12.write(155, 250, true);
}

void d3p(){
  s11.write(50, 200, true);
  delay(50);
  s11.write(130, 250, true);
}

void d3b(){
  s11.write(0, 200, true);
  delay(50);
  s11.write(130, 250, true);
}

void setup() {
  Serial.begin(9600);
  pinMode(7, INPUT_PULLUP);

  s11.attach(11);
  s12.attach(12);
  s13.attach(13);

  s11.write(180, 50, true);
  s12.write(180, 50, true);
  s13.write(180, 50, true);

  stepper.setMaxSpeed(20000);

  //----------- HOMING --------------

  stepper.setSpeed(4000);

  while (digitalRead(7) == HIGH) {//low é apertado
    stepper.runSpeed();
  }

  stepper.setCurrentPosition(0);

  stepper.setAcceleration(10000);//4000

}

float passos(float dist){//com base no motor 3
  return -((3200 * dist)/16.00);
}

float distancia(const char* tecla){
  //MAOZINHA 1
  if(tecla == "E1") return 30.70;
  if(tecla == "E2") return 44.00;

  //MAOZINHA 2
  
  if(tecla == "FA#1") return 6.60;
  if(tecla == "A") return 9.80;
  //if(tecla == "S#") return 8.80; nao usa
  if(tecla == "B") return 12.00;
  if(tecla == "D") return 15.30;
  if(tecla == "E") return 17.50;

  return -1;
}

void move(int dedo, const char* tecla){

  if(dedo == 1){
    stepper.runToNewPosition(passos(distancia(tecla) - dif1));

  } else if(dedo == 2){
    stepper.runToNewPosition(passos(distancia(tecla) - dif2));
    if(tecla == "A" or tecla == "B" or tecla == "D" or tecla == "E"){
      d2b();
    }
    if(tecla == "FA#1"){
      d2p();
    }

  } else if(dedo == 3){
    stepper.runToNewPosition(passos(distancia(tecla)));
    
    if(tecla == "A" or tecla == "B" or tecla == "D" or tecla == "E"){
      d3b();
    }
    if(tecla == "FA#1"){
      d3p();
    }
  }

}

void loop() {
  //s10.write(125,70, true);
  //delay(100);
  //s10.write(180, 70, true);
  //delay(100);
  //s10.write(180, 200, true);

  move(3, "FA#1");
  delay(500);
  
  

 /* FAZER FUNÇÃO
  move(1, "E2");
  d1b();
  move(1, "E1");
  d1p();

  */

  /*
  move(3, "A");
  move(3, "A");

  move(3, "D");
  move(3, "D");

  move(1, "E2");
  d1b();
  move(1, "E1");
  d1p();

  // making up
  move(3, "A");
  move(3, "A");

  move(3, "FA#1");
  move(3, "FA#1");

  // a song about
  move(3, "D");
  move(3, "D");

  //coraline
  move(3, "A");

  move(3, "FA#1");
  move(3, "FA#1");

  move(3, "D");
  move(3, "D");

  //she's a peach
  move(3, "A");
  move(3, "A");

  move(3, "FA#1");
  move(3, "FA#1");

  //she's a doll
  move(3, "D");
  move(3, "D");

  //she's a pal of mine

  move(3, "B");
  move(3, "B");

  move(3, "D");
  move(3, "D");

  move(3, "E");
  move(3, "E");



  delay(2000);*/


  //move(3, "A1");

  //d1b();*/

}