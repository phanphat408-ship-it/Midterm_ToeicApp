import 'package:flutter_test/flutter_test.dart';
import 'package:toeic_reading_app/main.dart';

void main() {
  testWidgets('ToeicReadingApp loads and displays header title', (WidgetTester tester) async {
    await tester.pumpWidget(const ToeicReadingApp());
    expect(find.text('TOEIC Reading Master'), findsOneWidget);
  });
}
