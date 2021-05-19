# Sistema Biofeedback para la Evalución del Nivel de Estrés
Este sistema fue desarrollado para analizar el nivel de estrés de una persona mediante sus caracteristcas fisiologicas como la temperatura corporal o de la piel, la frecuencia cardiaca y la hondas alpha y beta producidas por el cerebro.
Para este sistema utilizamos un sensor de temperatura (DS18B20), un sensor de electroencefalogra (Mindflex) ambos conectados a un Arduino Uno y un sensor de frecuencia cardiaca (MAX30102) conectado a un Arduino Nano.

### Pre-requisitos 📋

Antes de correr el sistema asegurarse de tener conectados los Arduinos con los sensores a su PC.

Codigo para sensor DS18B20
```
#include <OneWire.h>
#include <DallasTemperature.h>

OneWire ourWire(2); 
DallasTemperature sensors(&ourWire);

void setup() {
  delay(1000);
  Serial.begin(9600);
  //Se inicia el sensor
  sensors.begin();   
}

void loop() {
	//Se envía el comando para leer la temperatura
    sensors.requestTemperatures();   
	//Se obtiene la temperatura en ºC
    float temp= sensors.getTempCByIndex(0); 
    Serial.print("T,");
    Serial.println(temp);
	delay(100);
}
```

Codigo para Mindflex
```
#include <Brain.h>

Brain brain(Serial);

void setup() {
  Serial.begin(9600); 
}

void loop() {
  Serial.print("B,");
  Serial.println(brain.readCSV());
}
```

Codigo para MAX30102
```
#include <Wire.h>
#include "MAX30105.h"
#include "heartRate.h"

MAX30105 particleSensor;
const byte RATE_SIZE = 4; 
byte rates[RATE_SIZE];
byte rateSpot = 0;
long lastBeat = 0; 

float beatsPerMinute;
int beatAvg;

void setup()
{
  Serial.begin(9600);
  if (!particleSensor.begin(Wire, I2C_SPEED_FAST)) 
  {
    Serial.println("MAX30105 no encontrado");
    while (1);
  }
  particleSensor.setup(); 
  particleSensor.setPulseAmplitudeRed(0x0A); 
  particleSensor.setPulseAmplitudeGreen(0); 
}

void loop()
{
  long irValue = particleSensor.getIR();

  if (checkForBeat(irValue) == true)
  {
    //We sensed a beat!
    long delta = millis() - lastBeat;
    lastBeat = millis();

    beatsPerMinute = 60 / (delta / 1000.0);

    if (beatsPerMinute < 255 && beatsPerMinute > 20)
    {
      rates[rateSpot++] = (byte)beatsPerMinute; 
      rateSpot %= RATE_SIZE; 

      beatAvg = 0;
      for (byte x = 0 ; x < RATE_SIZE ; x++)
        beatAvg += rates[x];
      beatAvg /= RATE_SIZE;
    }
  }

  Serial.print(irValue);
  Serial.print(",");
  Serial.print(beatsPerMinute);
  Serial.print(",");
  Serial.print(beatAvg);

  if (irValue < 50000)
    Serial.print(",NoFinger");

  Serial.println();
}
```

### Manejo del Software

Una vez corriendo el sistema selecione los puertos COM que usará, para ello debe dar clic en ⚙️ (Configuración) de la primera pantalla, se le presentara una pantalla donde se le mostrara los puertos COM actules de su PC, selecione uno para Arduino UNO y otro para Arduino NANO para finalizar haga clic en "Aceptar".

A continuación, cree un usuario o persona a evaluar, despues de registrar sus datos haga clic en el botón "Registrar Sesión". Se le presentará la pantalla de registro de sesión donde se mostrarán los datos que actualmente están leyendo los sensores (En caso que haya selecionado mal los puertos COM le aparecerá un mensaje diciendo que no se puede leer los Arduinos, haga clic en "Regresar" y revise sus puertos COM). 

Ingrese un comentario sobre la sesión actual y de clic en el botón "Iniciar Sesion" cuando considere que desea terminar la sesión de clic en el botón "Terminar", esto dara por finaliazada la sesión.

Para revisar las sesiones de un usuario, selecionelo en la pantalla de "Usuarios", y selecione la sesión que desea ver, se le presentara un pantalla con graficos de los resultados de la sesión.


## Construido con 🛠️

Este proyecto lo construimos con:

* [Micrisoft Visual Studio](https://visualstudio.microsoft.com/es/) - Entorno de desarrollo usado
* [.Net](https://visualstudio.microsoft.com/es/vs/features/net-development/) - Desarrollado con WPF

## Autores ✒️

Las siguientes personas son aquellas que contribuyeron a estre proyecto:

* **Ivett Sangiacomo Pinto** - *Tesista* 
* **Juan Carlos Zuñiga Torres** - *Ascesor* 



