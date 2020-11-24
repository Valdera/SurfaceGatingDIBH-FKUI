#include <Adafruit_VL53L0X.h>
/*
  Install this library first in arduino: https://github.com/adafruit/Adafruit_VL53L0X
*/


// address we will assign if dual sensor is present
#define LOX1_ADDRESS 0x30
#define LOX2_ADDRESS 0x31
#define LOX3_ADDRESS 0x32

// set the pins to shutdown
#define SHT_LOX1 7
#define SHT_LOX2 6
#define SHT_LOX3 5

const int pushButton = 8;
const int LED = 9;
int rate = 0;
int pusher = 0;
int state = 0;
String text;

// objects for the vl53l0x
Adafruit_VL53L0X lox1 = Adafruit_VL53L0X();
Adafruit_VL53L0X lox2 = Adafruit_VL53L0X();
Adafruit_VL53L0X lox3 = Adafruit_VL53L0X();

// this holds the measurement
VL53L0X_RangingMeasurementData_t measure1;
VL53L0X_RangingMeasurementData_t measure2;
VL53L0X_RangingMeasurementData_t measure3;

 
void setID() {
  // all reset
  digitalWrite(SHT_LOX1, LOW);    
  digitalWrite(SHT_LOX2, LOW);
  digitalWrite(SHT_LOX3, LOW);
  delay(10);
  // all unreset
  digitalWrite(SHT_LOX1, HIGH);
  digitalWrite(SHT_LOX2, HIGH);
  digitalWrite(SHT_LOX3, HIGH);
  delay(10);

  // activating LOX1 and reseting LOX2
  digitalWrite(SHT_LOX1, HIGH);
  digitalWrite(SHT_LOX2, LOW);
  digitalWrite(SHT_LOX3, LOW);

  // initing LOX1
  if(!lox1.begin(LOX1_ADDRESS)) {
    while(1);
  }
  
  delay(10);
  // activating LOX2
  digitalWrite(SHT_LOX2, HIGH);
  delay(10);

  //initing LOX2
  if(!lox2.begin(LOX2_ADDRESS)) {
    while(1);
  }
  
  delay(10);
  digitalWrite(SHT_LOX3, HIGH);
  delay(10);
  
 //initing LOX2
  if(!lox3.begin(LOX3_ADDRESS)) {
    while(1);
  }
}

void addValue(int value){
  text = text + String(value) + ";";
}

void read_dual_sensors() {
  int measure_1 = 0;
  int measure_2 = 0;
  int measure_3 = 0;
  text = "";
  rate = 0;
  
  lox1.rangingTest(&measure1, false); // pass in 'true' to get debug data printout!
  lox2.rangingTest(&measure2, false); // pass in 'true' to get debug data printout!
  lox3.rangingTest(&measure3, false); // pass in 'true' to get debug data printout!

  state = digitalRead(pushButton);
  if(state == LOW){
    delay(10);
    pusher = 1;
    digitalWrite(LED, HIGH);
    if(state == LOW ){
      pusher = 1;
    }
  } else {
    digitalWrite(LED, LOW);
    pusher = 0;
  }

    measure_1 = measure1.RangeMilliMeter;
    rate = rate + measure_1;
    addValue(measure_1);
  

    measure_2 = measure2.RangeMilliMeter;
    rate = rate + measure_2;
    addValue(measure_2);

    measure_3 = measure3.RangeMilliMeter;
    rate = rate + measure_3;
    addValue(measure_3);

  
   addValue(rate/3);
   addValue(pusher);
    Serial.println(text);
 }

void setup() {
  Serial.begin(9600);

  // wait until serial port opens for native USB devices
  while (! Serial) { delay(1); }


  pinMode(SHT_LOX1, OUTPUT);
  pinMode(SHT_LOX2, OUTPUT);
  pinMode(SHT_LOX3, OUTPUT);


  digitalWrite(SHT_LOX3, LOW);
  digitalWrite(SHT_LOX1, LOW);
  digitalWrite(SHT_LOX2, LOW);

  pinMode(LED, OUTPUT);
  pinMode(pushButton, INPUT_PULLUP);
  setID();
}

void loop() {
  read_dual_sensors();
  delay(1000);
}
