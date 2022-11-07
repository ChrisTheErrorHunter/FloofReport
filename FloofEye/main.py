# This is a sample Python script.

# Press Shift+F10 to execute it or replace it with your code.
# Press Double Shift to search everywhere for classes, files, tool windows, actions, and settings.
import cv2
import psycopg2

# Press the green button in the gutter to run the script.
if __name__ == '__main__':
    cap = cv2.VideoCapture("C:/Users/Krzysztof/Documents/InzTransfer/2022-11-06/22-31-11.mp4")

    object_detector = cv2.createBackgroundSubtractorMOG2(history=100, varThreshold=40)

    currentLocation = "Nowhere"

    DATABASE_HOST = '192.168.1.33';
    DATABASE_USER = 'postgres';
    DATABASE_PASSWORD = 'Flafik,456';
    DATABASE_NAME = 'HamsterBook';

    conn = psycopg2.connect(
        dbname=DATABASE_NAME,
        user=DATABASE_USER,
        host=DATABASE_HOST,
        password=DATABASE_PASSWORD
    )

    cur = conn.cursor()
    cur.execute("SELECT version();")
    print(cur.fetchone()[0])
    cur.close()

    conn.close()
    while True:
        ret, frame = cap.read()
        height, width, _ = frame.shape
        aoi = frame[0 : 720, 50 : 1280]
        frame = aoi;
        mask = object_detector.apply(frame)
        aoi2 = frame[200:350, 220:350]
        miska = frame[350:500, 270:360]
        _, mask = cv2.threshold(mask, 254, 255, cv2.THRESH_BINARY)

        contours, _ = cv2.findContours(mask, cv2.RETR_TREE, cv2.CHAIN_APPROX_SIMPLE)
        for cnt in contours:
            area = cv2.contourArea(cnt)
            if 40 < area < 5000:
                if 220 < cnt[0][0][0] < 350 and 200 < cnt[0][0][1] < 350:
                    if currentLocation != "MICHA":
                        currentLocation = "MICHA"
                        print("MICHA")
                #print(cnt[0][0])
                cv2.drawContours(frame, [cnt], -1, (0, 255, 0), 2)
        #cv2.imshow("Framee", frame)
        cv2.imshow("aoi", aoi2)
        cv2.imshow("Mask", mask)
        cv2.imshow("Micha", frame)
        key = cv2.waitKey(20)
        #if key == 27:
            #break
    cap.release()
    cv2.destroyAllWindows()

# See PyCharm help at https://www.jetbrains.com/help/pycharm/
