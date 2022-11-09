# This is a sample Python script.

# Press Shift+F10 to execute it or replace it with your code.
# Press Double Shift to search everywhere for classes, files, tool windows, actions, and settings.
import cv2
import psycopg2
from datetime import datetime

# Press the green button in the gutter to run the script.
if __name__ == '__main__':
    cap = cv2.VideoCapture("C:/Users/mrkri/Desktop/InzPyt/GoodSample.mp4")

    object_detector = cv2.createBackgroundSubtractorMOG2(history=100, varThreshold=40)

    currentLocation = 0
    prevLocation = "Nowhere"

    DATABASE_HOST = '79.163.161.226';
    DATABASE_USER = 'postgres';
    DATABASE_PASSWORD = 'Flafik,456';
    DATABASE_NAME = 'HamsterBook';

    MichaMinX = 200
    MichaMinY = 220
    MichaMaxX = 350
    MichaMaxY = 350
    DomekMinX = 0
    DomekMinY = 650
    DomekMaxX = 170
    DomekMaxY = 900
    KoszyczekMinX = 70
    KoszyczekMinY = 900
    KoszyczekMaxX = 400
    KoszyczekMaxY = 1280
    KolkoMinX = 450
    KolkoMinY = 300
    KolkoMaxX = 720
    KolkoMaxY = 800

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
        aoi2 = frame[450:720, 300:800]
        _, mask = cv2.threshold(mask, 254, 255, cv2.THRESH_BINARY)

        contours, _ = cv2.findContours(mask, cv2.RETR_TREE, cv2.CHAIN_APPROX_SIMPLE)
        for cnt in contours:
            area = cv2.contourArea(cnt)
            if 40 < area < 5000:
                tmp = (where_is_outline(cnt[0][0][1], cnt[0][0][0]))
                if 0 != tmp != currentLocation:
                    now = datetime.now()
                    currentLocation = tmp
                    cur = conn.cursor()
                    cur.execute('''INSERT INTO visualevents (registrationtime, cageid, hamsterid, areaid) VALUES ('2022-10-10', 1, 1, %d);''' %(currentLocation))
                    conn.commit()
                    print('''Saved %d area into db''' %(currentLocation))
                    cur.close()
                #print(cnt[0][0])
                cv2.drawContours(frame, [cnt], -1, (0, 255, 0), 2)
        #cv2.imshow("Framee", frame)
        cv2.imshow("aoi", aoi2)
        #cv2.imshow("Mask", mask)
        cv2.imshow("Micha", frame)
        key = cv2.waitKey(1)
        #if key == 27:
            #break
    cap.release()
    cv2.destroyAllWindows()
    conn.close()

# See PyCharm help at https://www.jetbrains.com/help/pycharm/
