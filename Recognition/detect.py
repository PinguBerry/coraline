# python yolo_detect.py --model=yolo11n_ncnn_model --source=usb0 --resolution=640x480

# ========================= LIBRARIES =========================

import os
import sys
import argparse
import glob
import time
import serial
import RPi.GPIO as GPIO

import cv2
import numpy as np
from ultralytics import YOLO
from webcam_stream import WebcamStream


# ========================= MODEL INITIALIZATION =========================

# Load the trained YOLO object detection model
model = YOLO('botinha2_ncnn_model', task='detect')

# Retrieve class labels from the model
labels = model.names

# ========================= CAMERA INITIALIZATION =========================

# Start webcam stream
stream = WebcamStream(
    src=0,
    width=640,
    height=320,
    fps=20
).start()

# Allow camera time to initialize
time.sleep(1)

# Define colors for bounding boxes
bbox_colors = [
    (164,120,87), (68,148,228), (93,97,209),
    (178,182,133), (88,159,106), (96,202,231),
    (159,124,168), (169,162,241), (98,118,150),
    (172,176,184)
]



# ========================= PERFORMANCE VARIABLES =========================


avg_frame_rate = 0
frame_rate_buffer = []
fps_avg_len = 200
img_count = 0

cont = 0
starttime = time.time()
fps = 0
frame_count = 0
object_count = 0


# Configure Raspberry Pi GPIO
GPIO.setmode(GPIO.BOARD)

# Configure input pin
GPIO.setup(18, GPIO.IN)

# Initialize serial communication with Arduino Uno
ser = serial.Serial('/dev/ttyAMA0', 115200, timeout=1)

# ========================= LOOP =========================

while True:

    # Capture frame from camera
    frame = stream.read()

    frame_count += 1
    cont += 1

    # Calculate elapsed time for FPS estimation
    passou = time.time() - starttime

    # Run inference every 10 frames to reduce processing load
    if frame_count % 10 == 0:

        # Execute object detection
        results = model(frame, verbose=False)

        # Extract detected objects
        detections = results[0].boxes

        # Reset object counter
        object_count = 0

        # Process each detected object
        for i in range(len(detections)):

            # Extract bounding box coordinates
            xyxy_tensor = detections[i].xyxy.cpu()
            xyxy = xyxy_tensor.numpy().squeeze()

            xmin, ymin, xmax, ymax = xyxy.astype(int)

            # Calculate object center position
            centerx = (xmin + xmax) // 2

            # Retrieve detected class
            classidx = int(detections[i].cls.item())
            classname = labels[classidx]

            # Retrieve confidence score
            conf = detections[i].conf.item()

            # Only process detections above confidence threshold
            if conf > 0.8:

                # Draw bounding box
                color = bbox_colors[classidx % 10]
                cv2.rectangle(frame, (xmin, ymin), (xmax, ymax), color, 2)

                # Create detection label
                label = f'{classname}: {int(conf * 100)}%'

                # Draw label background and text
                labelSize, baseLine = cv2.getTextSize(label,cv2.FONT_HERSHEY_SIMPLEX,0.5,1)

                label_ymin = max(ymin, labelSize[1] + 10)

                cv2.rectangle(frame,(xmin, label_ymin - labelSize[1] - 10),(xmin + labelSize[0], label_ymin + baseLine - 10),color,cv2.FILLED)

                cv2.putText(frame,label,(xmin, label_ymin - 7),cv2.FONT_HERSHEY_SIMPLEX,0.5,(0, 0, 0),1)

                object_count += 1

                print(centerx)

                
                # ========================= OBJECT POSITION ANALYSIS =========================
                # Divide the camera image into three regions: Left, Center and Right.
                # Send a command to the Arduino according to the detected object's position.

                if centerx >= 0 and centerx < 213:

                    cv2.putText(frame,'left',(10, 60),cv2.FONT_HERSHEY_SIMPLEX,.7,(255,170,228),2)

                    print("left")

                    # Send left-position command
                    ser.write(b"1")

                if centerx >= 213 and centerx <= 427:

                    cv2.putText(frame, 'center', (10, 60),cv2.FONT_HERSHEY_SIMPLEX,.7, (255,170,228),2)

                    print("center")

                    # Send center-position command
                    ser.write(b"2")

                if centerx > 427 and centerx <= 640:

                    cv2.putText(frame,'right',(10, 60),cv2.FONT_HERSHEY_SIMPLEX,.7,(255,170,228),2)

                    print("right")

                    # Send right-position command
                    ser.write(b"3")


    # Update FPS every second
    if passou >= 1.0:

        fps = cont / passou

        cont = 0
        starttime = time.time()

    # Display current FPS
    cv2.putText(frame,f"FPS: {fps:.2f}",(10, 30),cv2.FONT_HERSHEY_SIMPLEX,1,(0, 255, 0),2)

    # Display total number of detected objects
    cv2.putText(frame,f'Number of objects: {object_count}', (10, 40), cv2.FONT_HERSHEY_SIMPLEX,7,(0,255,255),2)

    # Show detection results
    cv2.imshow('YOLO detection results', frame)

    # Exit program when 'q' is pressed
    if cv2.waitKey(1) & 0xFF == ord('q'):
        break

#  ========================= CLEANUP  =========================

# Stop camera stream and close display windows
stream.stop()
cv2.destroyAllWindows()