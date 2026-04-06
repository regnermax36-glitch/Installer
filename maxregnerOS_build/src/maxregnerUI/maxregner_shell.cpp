#include <QtWidgets/QApplication>
#include <QtWidgets/QWidget>
#include <QtWidgets/QHBoxLayout>
#include <QtWidgets/QPushButton>
#include <QtWidgets/QLabel>
#include <QtGui/QPainter>
#include <QtGui/QLinearGradient>
#include <QtCore/QDateTime>
#include <QtCore/QTimer>

class MaxregnerShell : public QWidget {
public:
    MaxregnerShell() {
        setWindowFlags(Qt::FramelessWindowHint | Qt::WindowStaysOnTopHint);
        setAttribute(Qt::WA_X11NetWmWindowTypeDock);
        setAttribute(Qt::WA_TranslucentBackground);
        resize(1280, 80);

        QHBoxLayout *l = new QHBoxLayout(this);
        l->setContentsMargins(20, 10, 20, 10);

        QPushButton *b = new QPushButton("maxregner 2029");
        b->setStyleSheet("background: qlineargradient(x1:0, y1:0, x2:1, y2:1, stop:0 rgba(0, 210, 255, 200), stop:1 rgba(58, 123, 213, 200)); "
                         "color: white; border-radius: 20px; font-weight: bold; padding: 10px 25px; border: 1px solid rgba(255,255,255,80);");
        l->addWidget(b);

        QStringList icons = {"🌐", "📂", "🎵", "💬", "🛠️"};
        for(const QString &icon : icons) {
            QPushButton *appBtn = new QPushButton(icon);
            appBtn->setFixedSize(50, 50);
            appBtn->setStyleSheet("background: rgba(255, 255, 255, 30); color: white; border-radius: 12px; font-size: 20px; border: 1px solid rgba(255,255,255,40);");
            l->addWidget(appBtn);
        }

        l->addStretch();

        QLabel *c = new QLabel();
        c->setStyleSheet("color: white; font-size: 22px; font-weight: 300; font-family: 'Segoe UI Light', sans-serif;");
        l->addWidget(c);

        QTimer *t = new QTimer(this);
        connect(t, &QTimer::timeout, [=](){ c->setText(QDateTime::currentDateTime().toString("HH:mm:ss")); });
        t->start(1000);
    }
protected:
    void paintEvent(QPaintEvent *) {
        QPainter p(this);
        p.setRenderHint(QPainter::Antialiasing);
        p.setBrush(QColor(15, 15, 15, 140));
        p.setPen(QPen(QColor(255, 255, 255, 50), 1));
        p.drawRoundedRect(rect().adjusted(1, 1, -1, -1), 25, 25);

        QLinearGradient grad(0, 0, 0, height());
        grad.setColorAt(0, QColor(255, 255, 255, 30));
        grad.setColorAt(0.5, QColor(255, 255, 255, 0));
        p.setBrush(grad);
        p.drawRoundedRect(rect().adjusted(1, 1, -1, -1), 25, 25);
    }
};

int main(int argc, char *argv[]) {
    QApplication a(argc, argv);
    MaxregnerShell s;
    s.show();
    return a.exec();
}
