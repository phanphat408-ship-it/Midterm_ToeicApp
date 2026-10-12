import 'package:flutter/material.dart';
import 'package:flutter/services.dart';
import 'screens/exam_list_screen.dart';
import 'theme/app_theme.dart';

void main() {
  WidgetsFlutterBinding.ensureInitialized();
  SystemChrome.setSystemUIOverlayStyle(
    const SystemUiOverlayStyle(
      statusBarColor: Colors.transparent,
      statusBarIconBrightness: Brightness.light,
    ),
  );
  runApp(const ToeicReadingApp());
}

class ToeicReadingApp extends StatelessWidget {
  const ToeicReadingApp({super.key});

  @override
  Widget build(BuildContext context) {
    return MaterialApp(
      title: 'TOEIC Reading Master',
      debugShowCheckedModeBanner: false,
      theme: AppTheme.lightTheme,
      home: const ExamListScreen(),
    );
  }
}
