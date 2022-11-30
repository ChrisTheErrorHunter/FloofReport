# This is a sample Python script.

# Press Shift+F10 to execute it or replace it with your code.
# Press Double Shift to search everywhere for classes, files, tool windows, actions, and settings.
import string
import time
import cv2
import psycopg2
import socket
import subprocess
import sys
import shutil
from datetime import datetime, timedelta
from os import listdir


OFFSETX = 20
OFFSETY = 0
MichaMinX = 230 + OFFSETX
MichaMinY = 200 + OFFSETY
MichaMaxX = 370 + OFFSETX
MichaMaxY = 330 + OFFSETY
DomekMinX = 0 + OFFSETX
DomekMinY = 630 + OFFSETY
DomekMaxX = 190 + OFFSETX
DomekMaxY = 860 + OFFSETY
KoszyczekMinX = 70 + OFFSETX
KoszyczekMinY = 900 + OFFSETY
KoszyczekMaxX = 500 + OFFSETX
KoszyczekMaxY = 1280 + OFFSETY
KolkoMinX = 450 + OFFSETX
KolkoMinY = 200 + OFFSETY
KolkoMaxX = 720 + OFFSETX
KolkoMaxY = 700 + OFFSETY
PoidloMinX = 140 + OFFSETX
PoidloMinY = 0 + OFFSETY
PoidloMaxX = 290 + OFFSETX
PoidloMaxY = 130 + OFFSETY
SloikMinX = 400 + OFFSETX
SloikMinY = 0 + OFFSETY
SloikMaxX = 620 + OFFSETX
SloikMaxY = 150 + OFFSETY
CentrumMinX = 450 + OFFSETX
CentrumMinY = 200 + OFFSETY
CentrumMaxX = 720 + OFFSETX
CentrumMaxY = 700 + OFFSETY
FrontMinX = 450 + OFFSETX
FrontMinY = 200 + OFFSETY
FrontMaxX = 720 + OFFSETX
FrontMaxY = 700 + OFFSETY
DebugMinX = 400 + OFFSETX
DebugMinY = 0 + OFFSETY
DebugMaxX = 620 + OFFSETX
DebugMaxY = 150 + OFFSETY

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
        return 5
    else:
        return 0

def Analise(file_path):
    cap = cv2.VideoCapture(sys.argv[1])
    frame_num = 0
    fps = cap.get(cv2.CAP_PROP_FPS)
    proces = subprocess.Popen(['./date_of_birth.sh', sys.argv[1]], stdout=subprocess.PIPE)
    output, _ = proces.communicate()
    output = output.decode('ascii')
    print(output)
    raw_time = output
    # raw_time = '2022-11-23 04:34:56.5611'
    FDOB = datetime.strptime(raw_time, '%Y-%m-%d %H:%M:%S.%f')
    print(fps, "FPS")
    milesecound_per_frame = 1000 / fps

    object_detector = cv2.createBackgroundSubtractorMOG2(history=100, varThreshold=40)

    currentLocation = 0
    prevLocation = "Nowhere"
    print("Db Address", socket.gethostbyname("mulawa.ddns.net"))
    DATABASE_HOST = socket.gethostbyname("mulawa.ddns.net")
    DATABASE_USER = 'postgres'
    DATABASE_PASSWORD = 'Flafik,456'
    DATABASE_NAME = 'HamsterBook'

    conn = psycopg2.connect(
        dbname=DATABASE_NAME,
        user=DATABASE_USER,
        host=DATABASE_HOST,
        password=DATABASE_PASSWORD
    )

    # cur = conn.cursor()
    # cur.execute("SELECT version();")
    # print(cur.fetchone()[0])
    # cur.close()

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
        contours, _ = cv2.findContours(mask, cv2.RETR_TREE, cv2.CHAIN_APPROX_SIMPLE)
        if (len(contours) == 0):
            continue
        cnt = max(contours, key=cv2.contourArea)
        area = cv2.contourArea(cnt)
        if 40 < area < 5000:
            tmp = (where_is_outline(cnt[0][0][1], cnt[0][0][0]))
            if 0 != tmp != currentLocation:
                time_to_insert = FDOB + timedelta(milliseconds=milesecound_per_frame) * frame_num
                currentLocation = tmp
                cur = conn.cursor()
                cur.execute(
                    '''INSERT INTO visualevents (registrationtime, cageid, hamsterid, areaid) VALUES ('%s', 1, 1, %d);''' % (
                    time_to_insert, currentLocation))
                conn.commit()
                print('''Saved %d area into db''' % (currentLocation))
                cur.close()
                # print(cnt[0][0])
            cv2.drawContours(frame, [cnt], -1, (0, 255, 0), 2)
        # cv2.imshow("Framee", frame)
        # cv2.imshow("aoi", aoi2)
        # cv2.imshow("Mask", mask)
        # cv2.imshow("Micha", frame)
        # key = cv2.waitKey(1)
        frame_num += 1
        # if key == 27:
        # break
    cap.release()
    cv2.destroyAllWindows()
    conn.close()

# Press the green button in the gutter to run the script.
if __name__ == '__main__':
    #Analise(sys.argv[1])
    while True:
        files = listdir('./')
        time.sleep(3)
        for f in files:
            if f.endswith('.thumb'):
	            movie = f[:len(f) - 6]
	            print(movie)
	            Analise(movie)
	            #shutil.move(movie, './WorkingDirectory')

# See PyCharm help at https://www.jetbrains.com/help/pycharm/
