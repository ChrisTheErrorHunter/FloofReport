# This is a sample Python script.

# Press Shift+F10 to execute it or replace it with your code.
# Press Double Shift to search everywhere for classes, files, tool windows, actions, and settings.
import string

import cv2
import psycopg2
import socket
from datetime import datetime, timedelta

# Press the green button in the gutter to run the script.
if __name__ == '__main__':
    cap = cv2.VideoCapture("C:/Users/mrkri/Videos/v1123/04-39-29.mp4")
    frame_num = 0
    fps = cap.get(cv2.CAP_PROP_FPS)
    raw_time = '2022-11-23 04:34:56.5611'
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

    OFFSETX = 40
    OFFSETY = 35

    MichaMinX = 200 + OFFSETX
    MichaMinY = 220 + OFFSETY
    MichaMaxX = 350 + OFFSETX
    MichaMaxY = 350 + OFFSETY
    DomekMinX = 0 + OFFSETX
    DomekMinY = 650 + OFFSETY
    DomekMaxX = 170 + OFFSETX
    DomekMaxY = 900 + OFFSETY
    KoszyczekMinX = 70 + OFFSETX
    KoszyczekMinY = 900 + OFFSETY
    KoszyczekMaxX = 400 + OFFSETX
    KoszyczekMaxY = 1280 + OFFSETY
    KolkoMinX = 450 + OFFSETX
    KolkoMinY = 300 + OFFSETY
    KolkoMaxX = 720 + OFFSETX
    KolkoMaxY = 800 + OFFSETY

    def where_is_outline(x: int, y: int) -> int:
        if (MichaMinX < x < MichaMaxX) and (MichaMinY < y < MichaMaxY):
            return 1
        elif (DomekMinX < x < DomekMaxX) and (DomekMinY < y < DomekMaxY):
            return 2
        elif (KoszyczekMinX < x < KoszyczekMaxX) and (KoszyczekMinY < y < KoszyczekMaxY):
            return 3
        elif (KolkoMinX < x < KolkoMaxX) and (KolkoMinY < y < KolkoMaxY):
            return 4
        else:
            return 0

    conn = psycopg2.connect(
        dbname=DATABASE_NAME,
        user=DATABASE_USER,
        host=DATABASE_HOST,
        password=DATABASE_PASSWORD
    )

    #cur = conn.cursor()
    #cur.execute("SELECT version();")
    #print(cur.fetchone()[0])
    #cur.close()

    while True:
        ret, frame = cap.read()
        height, width, _ = frame.shape
        aoi = frame[0 : 720, 50 : 1280]
        frame = aoi;
        mask = object_detector.apply(frame)
        aoi2 = frame[DomekMinX:DomekMaxX, DomekMinY:DomekMaxY]
        _, mask = cv2.threshold(mask, 254, 255, cv2.THRESH_BINARY)
        contours, _ = cv2.findContours(mask, cv2.RETR_TREE, cv2.CHAIN_APPROX_SIMPLE)
        for cnt in contours:
            area = cv2.contourArea(cnt)
            if 40 < area < 5000:
                tmp = (where_is_outline(cnt[0][0][1], cnt[0][0][0]))
                if 0 != tmp != currentLocation:
                    time_to_insert = FDOB + timedelta(milliseconds=milesecound_per_frame) * frame_num
                    currentLocation = tmp
                    cur = conn.cursor()
                    cur.execute('''INSERT INTO visualevents (registrationtime, cageid, hamsterid, areaid) VALUES ('%s', 1, 1, %d);''' %(time_to_insert, currentLocation))
                    conn.commit()
                    print('''Saved %d area into db''' %(currentLocation))
                    cur.close()
                #print(cnt[0][0])
                cv2.drawContours(frame, [cnt], -1, (0, 255, 0), 2)
        #cv2.imshow("Framee", frame)
        cv2.imshow("aoi", aoi2)
        cv2.imshow("Mask", mask)
        cv2.imshow("Micha", frame)
        key = cv2.waitKey(1)
        frame_num += 1
        #if key == 27:
            #break
    cap.release()
    cv2.destroyAllWindows()
    conn.close()

# See PyCharm help at https://www.jetbrains.com/help/pycharm/
