import string
import time
import cv2
import psycopg2
import socket
import subprocess
import configparser
import sys
import shutil
import os
from datetime import datetime, timedelta
from os import listdir

config = configparser.ConfigParser()
config.read('./FloofConfig.conf')
archive_path = config['INNER_SYSTEM_PATHS']['archive']
processing_path = config['INNER_SYSTEM_PATHS']['processing']

OFFSETX = 0
OFFSETY = 0
MichaMinX = int(config['CAGE_AREAS']['MichaMinX']) + OFFSETX
MichaMinY = int(config['CAGE_AREAS']['MichaMinY']) + OFFSETY
MichaMaxX = int(config['CAGE_AREAS']['MichaMaxX']) + OFFSETX
MichaMaxY = int(config['CAGE_AREAS']['MichaMaxY']) + OFFSETY
DomekMinX = int(config['CAGE_AREAS']['DomekMinX']) + OFFSETX
DomekMinY = int(config['CAGE_AREAS']['DomekMinY']) + OFFSETY
DomekMaxX = int(config['CAGE_AREAS']['DomekMaxX']) + OFFSETX
DomekMaxY = int(config['CAGE_AREAS']['DomekMaxY']) + OFFSETY
KoszyczekMinX = int(config['CAGE_AREAS']['KoszyczekMinX']) + OFFSETX
KoszyczekMinY = int(config['CAGE_AREAS']['KoszyczekMinY']) + OFFSETY
KoszyczekMaxX = int(config['CAGE_AREAS']['KoszyczekMaxX']) + OFFSETX
KoszyczekMaxY = int(config['CAGE_AREAS']['KoszyczekMaxY']) + OFFSETY
KolkoMinX = int(config['CAGE_AREAS']['KolkoMinX']) + OFFSETX
KolkoMinY = int(config['CAGE_AREAS']['KolkoMinY']) + OFFSETY
KolkoMaxX = int(config['CAGE_AREAS']['KolkoMaxX']) + OFFSETX
KolkoMaxY = int(config['CAGE_AREAS']['KolkoMaxY']) + OFFSETY
PoidloMinX = int(config['CAGE_AREAS']['PoidloMinX']) + OFFSETX
PoidloMinY = int(config['CAGE_AREAS']['PoidloMinY']) + OFFSETY
PoidloMaxX = int(config['CAGE_AREAS']['PoidloMaxX']) + OFFSETX
PoidloMaxY = int(config['CAGE_AREAS']['PoidloMaxY']) + OFFSETY
SloikMinX = int(config['CAGE_AREAS']['SloikMinX']) + OFFSETX
SloikMinY = int(config['CAGE_AREAS']['SloikMinY']) + OFFSETY
SloikMaxX = int(config['CAGE_AREAS']['SloikMaxX']) + OFFSETX
SloikMaxY = int(config['CAGE_AREAS']['SloikMaxY']) + OFFSETY
MostekMinX = int(config['CAGE_AREAS']['MostekMinX']) + OFFSETX
MostekMinY = int(config['CAGE_AREAS']['MostekMinY']) + OFFSETY
MostekMaxX = int(config['CAGE_AREAS']['MostekMaxX']) + OFFSETX
MostekMaxY = int(config['CAGE_AREAS']['MostekMaxY']) + OFFSETY
CentrumMinX = int(config['CAGE_AREAS']['CentrumMinX']) + OFFSETX
CentrumMinY = int(config['CAGE_AREAS']['CentrumMinY']) + OFFSETY
CentrumMaxX = int(config['CAGE_AREAS']['CentrumMaxX']) + OFFSETX
CentrumMaxY = int(config['CAGE_AREAS']['CentrumMaxY']) + OFFSETY
FrontMinX = int(config['CAGE_AREAS']['FrontMinX']) + OFFSETX
FrontMinY = int(config['CAGE_AREAS']['FrontMinY']) + OFFSETY
FrontMaxX = int(config['CAGE_AREAS']['FrontMaxX']) + OFFSETX
FrontMaxY = int(config['CAGE_AREAS']['FrontMaxY']) + OFFSETY
DebugMinX = MostekMinX
DebugMinY = MostekMinY
DebugMaxX = MostekMaxX
DebugMaxY = MostekMaxY

def floof_log(txt):
    with open("FloofLog.txt", "a") as file_object:
        file_object.write(txt+'\n')

def where_is_outline(x: int, y: int) -> int:
    if (MichaMinX < x < MichaMaxX) and (MichaMinY < y < MichaMaxY):
        return 1
    elif (DomekMinX < x < DomekMaxX) and (DomekMinY < y < DomekMaxY):
        return 2
    elif (KoszyczekMinX < x < KoszyczekMaxX) and (KoszyczekMinY < y < KoszyczekMaxY):
        return 3
    elif (KolkoMinX < x < KolkoMaxX) and (KolkoMinY < y < KolkoMaxY):
        return 4
    elif (PoidloMinX < x < PoidloMaxX) and (PoidloMinY < y < PoidloMaxY):
        return 5
    elif (SloikMinX < x < SloikMaxX) and (SloikMinY < y < SloikMaxY):
        return 6
    elif (MostekMinX < x < MostekMaxX) and (MostekMinY < y < MostekMaxY):
        return 9
    else:
        return 0

def Analise(file_path):
    cap = cv2.VideoCapture(file_path)
    frame_num = 0
    fps = cap.get(cv2.CAP_PROP_FPS)
    proces = subprocess.Popen(['./date_of_birth.sh', file_path], stdout=subprocess.PIPE)
    output, _ = proces.communicate()
    output = output.decode('ascii')
    now = datetime.now()
    dt_string = now.strftime("%d/%m/%Y %H:%M:%S")
    log_entry = "Began analising at server time: " + dt_string
    log_entry += ('\n' + "Analising file with date: " + output)
    raw_time = output
    FDOB = datetime.strptime(raw_time, '%Y-%m-%d %H:%M:%S.%f')
    log_entry += ('\nDetected FPS: ' + str(fps))
    floof_log(log_entry)
    milesecound_per_frame = 1000 / fps
    movementDetected = False

    object_detector = cv2.createBackgroundSubtractorMOG2(history=100, varThreshold=40)

    currentLocation = 0
    prevLocation = "Nowhere"
    log_entry = ("Db Address on ip: " +  socket.gethostbyname(config['DATABASE']['address']))
    floof_log(log_entry)
    DATABASE_HOST = socket.gethostbyname(config['DATABASE']['address'])
    DATABASE_USER = config['DATABASE']['user']
    DATABASE_PASSWORD = config['DATABASE']['password']
    DATABASE_NAME = config['DATABASE']['dbname']
    hamsterId = int(config['DATABASE']['hamsterid'])
    cageId = int(config['DATABASE']['cageid'])
    debugFeed = config['DEBUG_OPTIONS']['DebugFeed']
    saveFeed = config['DEBUG_OPTIONS']['SaveFeed']
    saveMovementOnly = config['DEBUG_OPTIONS']['SaveOnlyMovement']

    conn = psycopg2.connect(
        dbname=DATABASE_NAME,
        user=DATABASE_USER,
        host=DATABASE_HOST,
        password=DATABASE_PASSWORD
    )

    while True:
        ret, frame = cap.read()
        if frame is None:
            break
        height, width, _ = frame.shape
        aoi = frame[0: 720, 30: 1280]
        frame = aoi;
        mask = object_detector.apply(frame)
        aoi2 = frame[DebugMinX:DebugMaxX, DebugMinY:DebugMaxY]
        _, mask = cv2.threshold(mask, 254, 255, cv2.THRESH_BINARY)
        contours, _ = cv2.findContours(mask, cv2.RETR_EXTERNAL, cv2.CHAIN_APPROX_SIMPLE)
        if (len(contours) == 0):
            continue
        cnt = max(contours, key=cv2.contourArea)
        area = cv2.contourArea(cnt)
        if 40 < area < 5000 and frame_num > 2:
            tmp = (where_is_outline(cnt[0][0][1], cnt[0][0][0]))
            if 0 != tmp != currentLocation:
                movementDetected = True
                time_to_insert = FDOB + timedelta(milliseconds=milesecound_per_frame) * frame_num
                currentLocation = tmp
                cur = conn.cursor()
                cur.execute(
                    '''INSERT INTO visualevents (registrationtime, cageid, hamsterid, areaid) VALUES ('%s', %d, %d, %d);''' % (
                    time_to_insert, hamsterId, cageId, currentLocation))
                conn.commit()
                floof_log('''Saved %d area into db with time of: %s''' % (currentLocation, time_to_insert))
                cur.close()
                if debugFeed == '1':
                    cv2.drawContours(frame, [cnt], -1, (0, 255, 0), 2)
        if debugFeed == '1':
            cv2.imshow("Framee", frame)
            cv2.imshow("aoi", aoi2)
            cv2.imshow("Mask", mask)
            key = cv2.waitKey(1)
        frame_num += 1
    cap.release()
    cv2.destroyAllWindows()
    conn.close()
    if saveFeed == '1' and (saveMovementOnly == '0' or movementDetected == True):
        shutil.move(file_path, archive_path)
    else:
        os.remove(file_path)
    now = datetime.now()
    dt_string = now.strftime("%d/%m/%Y %H:%M:%S")
    floof_log("Ended work for upper file at server time: " + dt_string)

if __name__ == '__main__':
    while True:
        files = listdir('./')
        time.sleep(3)
        for f in files:
            if f.endswith('.thumb'):
	            movie = f[:len(f) - 6]
	            shutil.move(movie, processing_path)
	            Analise(processing_path + movie)
	            os.remove(f)

