#include <QtWidgets/QApplication>
#include <QtWidgets/QWidget>
#include <QtWidgets/QHBoxLayout>
#include <QtWidgets/QPushButton>
#include <QtWidgets/QLabel>
#include <QtGui/QPainter>
#include <QtCore/QDateTime>
#include <QtCore/QTimer>

class MaxregnerShell : public QWidget {
public:
    MaxregnerShell() {
        setWindowFlags(Qt::FramelessWindowHint | Qt::WindowStaysOnTopHint | Qt::X11NetWmWindowTypeDock);
        setAttribute(Qt::WA_TranslucentBackground);
        resize(1200, 60);
        QHBoxLayout *l = new QHBoxLayout(this);
        QPushButton *b = new QPushButton("maxregnerOS 2029");
        b->setStyleSheet("background: #00aaff; color: white; border-radius: 10px; font-weight: bold;");
        l->addWidget(b);
        l->addStretch();
        QLabel *c = new QLabel();
        c->setStyleSheet("color: white; font-size: 18px;");
        l->addWidget(c);
        QTimer *t = new QTimer(this);
        connect(t, &QTimer::timeout, [=](){ c->setText(QDateTime::currentDateTime().toString("HH:mm:ss")); });
        t->start(1000);
    }
protected:
    void paintEvent(QPaintEvent *) {
        QPainter p(this);
        p.setBrush(QColor(0, 0, 0, 150));
        p.setPen(Qt::NoPen);
        p.drawRoundedRect(rect(), 15, 15);
    }
};

int main(int argc, char *argv[]) {
    QApplication a(argc, argv);
    MaxregnerShell s;
    s.show();
    return a.exec();
}
