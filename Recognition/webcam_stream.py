#!/usr/bin/env python
# -*- coding: utf-8 -*-
#
# WebcamStream class for real-time video capture using a separate thread
#

import cv2
from threading import Thread

class WebcamStream:
    def __init__(self, src=0, width=640, height=640, fps=30):

        # Initialize video capture 
        self.cap = cv2.VideoCapture(src, cv2.CAP_V4L2)

        # Set camera resolution
        self.cap.set(cv2.CAP_PROP_FRAME_WIDTH, width)
        self.cap.set(cv2.CAP_PROP_FRAME_HEIGHT, height)

        # Set desired FPS (may depend on camera hardware support)
        self.cap.set(cv2.CAP_PROP_FPS, fps)

        # Read first frame to initialize stream state
        self.grabbed, self.frame = self.cap.read()

        # Control flag to stop the thread safely
        self.stopped = False

        # Store configuration parameters
        self.fps = fps
        self.width = width
        self.height = height

    def start(self):

        # Start background thread for continuous frame capture
        Thread(target=self.update, daemon=True).start()
        return self

    def update(self):

        # Continuously grab frames from the camera in a separate thread
        while not self.stopped:

            # Grab the latest frame from buffer
            if not self.cap.grab():
                continue

            # Retrieve the actual frame data
            self.grabbed, self.frame = self.cap.retrieve()

    def read(self):

        # Return the most recent captured frame
        return self.frame

    def stop(self):

        # Stop the capture loop and release camera resources
        self.stopped = True
        self.cap.release()
