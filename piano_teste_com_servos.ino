#include <AccelStepper.h>

#define STEP_PIN 3
#define DIR_PIN 4
#include <VarSpeedServo.h>
VarSpeedServo s8; // cria o objeto servo
int angulo = 0; // declara origem do angulo como 0°
VarSpeedServo s9;
VarSpeedServo s10;

AccelStepper stepper(AccelStepper::DRIVER, STEP_PIN, DIR_PIN);

void setup() {
    s8.attach(8); //declara pino digital utilizado
    s9.attach(9);
    s10.attach(10);
  stepper.setMaxSpeed(4000);      // alta velocidade (não tão importante aqui)
  stepper.setAcceleration(4000);  // 🔥 MUITO IMPORTANTE
  stepper.moveTo(50);            // distância pequena
}

void loop() {
  if (stepper.distanceToGo() == 0) {
    stepper.moveTo(-stepper.currentPosition()); // vai e volta rápido
    s8.write(180, 70, true);
    s8.write(70, 70, true);
    s9.write(180, 70, true);
    s9.write(70, 70, true);
    s10.write(180, 70, true);
    s10.write(70, 70, true);
  }

  stepper.run();
}