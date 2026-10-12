import 'dart:async';
import 'package:flutter/material.dart';
import '../models/exam_models.dart';
import '../models/submit_models.dart';
import '../services/api_service.dart';
import '../theme/app_theme.dart';
import '../widgets/question_grid_dialog.dart';
import 'exam_result_screen.dart';

class ExamTakingScreen extends StatefulWidget {
  final int examId;
  final String examTitle;

  const ExamTakingScreen({
    super.key,
    required this.examId,
    required this.examTitle,
  });

  @override
  State<ExamTakingScreen> createState() => _ExamTakingScreenState();
}

class _ExamTakingScreenState extends State<ExamTakingScreen> {
  ExamDetail? _examDetail;
  bool _isLoading = true;
  List<QuestionItem> _questions = [];
  int _currentIndex = 0;

  // Selected answers: map of questionId -> selectedOption ("A", "B", "C", "D" or null)
  final Map<int, String?> _answers = {};

  // Timer
  Timer? _countdownTimer;
  late int _remainingSeconds;
  DateTime _startTime = DateTime.now();

  @override
  void initState() {
    super.initState();
    _loadExam();
  }

  @override
  void dispose() {
    _countdownTimer?.cancel();
    super.dispose();
  }

  Future<void> _loadExam() async {
    setState(() => _isLoading = true);
    final detail = await ApiService().getExamDetail(widget.examId);
    final allQ = detail.getAllQuestions();

    setState(() {
      _examDetail = detail;
      _questions = allQ;
      _remainingSeconds = detail.durationMinutes * 60;
      _startTime = DateTime.now();
      _isLoading = false;
    });

    _startTimer();
  }

  void _startTimer() {
    _countdownTimer?.cancel();
    _countdownTimer = Timer.periodic(const Duration(seconds: 1), (timer) {
      if (_remainingSeconds > 0) {
        setState(() => _remainingSeconds--);
      } else {
        timer.cancel();
        _onTimeExpired();
      }
    });
  }

  void _onTimeExpired() {
    showDialog(
      context: context,
      barrierDismissible: false,
      builder: (ctx) => AlertDialog(
        shape: RoundedRectangleBorder(borderRadius: BorderRadius.circular(20)),
        title: const Row(
          children: [
            Icon(Icons.timer_off_rounded, color: AppColors.redError, size: 28),
            SizedBox(width: 8),
            Text('Hết giờ làm bài!', style: TextStyle(fontWeight: FontWeight.bold)),
          ],
        ),
        content: const Text(
          'Thời gian 75 phút đã kết thúc. Hệ thống sẽ tự động nộp bài và chuyển sang màn hình kết quả.',
          style: TextStyle(fontSize: 14),
        ),
        actions: [
          ElevatedButton(
            onPressed: () {
              Navigator.of(ctx).pop();
              _submitExam(autoSubmit: true);
            },
            style: ElevatedButton.styleFrom(backgroundColor: AppColors.primaryRoyalBlue),
            child: const Text('Xem kết quả ngay'),
          ),
        ],
      ),
    );
  }

  void _fastForwardTimerDemo() {
    setState(() {
      _remainingSeconds = 10;
    });
    ScaffoldMessenger.of(context).showSnackBar(
      const SnackBar(
        content: Text('⚡ Đã giả lập đồng hồ còn 10 giây để test tự động nộp bài!'),
        duration: Duration(seconds: 2),
        backgroundColor: AppColors.amberWarning,
      ),
    );
  }

  QuestionGroupItem? _getGroupForQuestion(QuestionItem q) {
    if (q.questionGroupId == null || _examDetail == null) return null;
    for (var part in _examDetail!.parts) {
      for (var grp in part.questionGroups) {
        if (grp.id == q.questionGroupId) return grp;
      }
    }
    return null;
  }

  void _jumpToPart(int partNumber) {
    for (int i = 0; i < _questions.length; i++) {
      final q = _questions[i];
      if (partNumber == 5 && q.questionNumber >= 101 && q.questionNumber <= 130) {
        setState(() => _currentIndex = i);
        return;
      } else if (partNumber == 6 && q.questionNumber >= 131 && q.questionNumber <= 146) {
        setState(() => _currentIndex = i);
        return;
      } else if (partNumber == 7 && q.questionNumber >= 147) {
        setState(() => _currentIndex = i);
        return;
      }
    }
  }

  void _selectOption(String opt) {
    final q = _questions[_currentIndex];
    setState(() {
      if (_answers[q.id] == opt) {
        _answers[q.id] = null; // deselect
      } else {
        _answers[q.id] = opt;
      }
    });
  }

  void _clearCurrentAnswer() {
    final q = _questions[_currentIndex];
    setState(() {
      _answers[q.id] = null;
    });
    ScaffoldMessenger.of(context).showSnackBar(
      const SnackBar(
        content: Text('Đã hủy lựa chọn câu hỏi này'),
        duration: Duration(milliseconds: 900),
      ),
    );
  }

  void _showQuestionGrid() {
    showModalBottomSheet(
      context: context,
      isScrollControlled: true,
      backgroundColor: Colors.transparent,
      builder: (ctx) => QuestionGridDialog(
        questions: _questions,
        userAnswers: _answers,
        currentIndex: _currentIndex,
        onSelectQuestion: (index) {
          setState(() => _currentIndex = index);
        },
      ),
    );
  }

  void _confirmSubmit() {
    int total = _questions.length;
    int answered = _answers.values.where((ans) => ans != null && ans.isNotEmpty).length;
    int unanswered = total - answered;

    showDialog(
      context: context,
      builder: (ctx) => AlertDialog(
        shape: RoundedRectangleBorder(borderRadius: BorderRadius.circular(20)),
        title: const Text(
          'Xác nhận nộp bài',
          style: TextStyle(fontWeight: FontWeight.bold, fontSize: 18),
        ),
        content: Column(
          mainAxisSize: MainAxisSize.min,
          crossAxisAlignment: CrossAxisAlignment.start,
          children: [
            if (unanswered > 0)
              Container(
                padding: const EdgeInsets.all(12),
                margin: const EdgeInsets.only(bottom: 12),
                decoration: BoxDecoration(
                  color: AppColors.amberLight,
                  borderRadius: BorderRadius.circular(10),
                  border: Border.all(color: AppColors.amberWarning.withValues(alpha: 0.5)),
                ),
                child: Row(
                  children: [
                    const Icon(Icons.warning_amber_rounded, color: AppColors.amberWarning),
                    const SizedBox(width: 8),
                    Expanded(
                      child: Text(
                        'Bạn còn $unanswered câu chưa hoàn thành (trên tổng số $total câu).',
                        style: const TextStyle(
                          fontSize: 13,
                          fontWeight: FontWeight.w600,
                          color: Color(0xFF92400E),
                        ),
                      ),
                    ),
                  ],
                ),
              ),
            const Text(
              'Bạn có chắc chắn muốn nộp bài thi ngay bây giờ không?',
              style: TextStyle(fontSize: 14),
            ),
          ],
        ),
        actions: [
          TextButton(
            onPressed: () => Navigator.of(ctx).pop(),
            child: const Text('Tiếp tục làm bài', style: TextStyle(color: AppColors.textMuted)),
          ),
          ElevatedButton(
            onPressed: () {
              Navigator.of(ctx).pop();
              _submitExam();
            },
            style: ElevatedButton.styleFrom(backgroundColor: AppColors.emeraldGreen),
            child: const Text('Nộp bài thi'),
          ),
        ],
      ),
    );
  }

  Future<void> _submitExam({bool autoSubmit = false}) async {
    _countdownTimer?.cancel();

    // Show loading indicator
    showDialog(
      context: context,
      barrierDismissible: false,
      builder: (ctx) => const Center(
        child: Card(
          child: Padding(
            padding: EdgeInsets.all(24),
            child: Column(
              mainAxisSize: MainAxisSize.min,
              children: [
                CircularProgressIndicator(color: AppColors.primaryRoyalBlue),
                SizedBox(height: 16),
                Text('Đang nộp bài và chấm điểm...'),
              ],
            ),
          ),
        ),
      ),
    );

    final payload = SubmitExamPayload(
      examId: widget.examId,
      studentName: 'Thí sinh TOEIC',
      startTime: _startTime,
      submitTime: DateTime.now(),
      answers: _questions.map((q) {
        return QuestionAnswerPayload(
          questionId: q.id,
          questionNumber: q.questionNumber,
          selectedOption: _answers[q.id],
        );
      }).toList(),
    );

    final result = await ApiService().submitExam(payload);

    if (mounted) {
      Navigator.of(context).pop(); // dismiss loading
      Navigator.of(context).pushReplacement(
        MaterialPageRoute(
          builder: (context) => ExamResultScreen(
            result: result,
            examTitle: widget.examTitle,
            questions: _questions,
          ),
        ),
      );
    }
  }

  @override
  Widget build(BuildContext context) {
    if (_isLoading) {
      return const Scaffold(
        backgroundColor: AppColors.background,
        body: Center(child: CircularProgressIndicator()),
      );
    }

    final q = _questions[_currentIndex];
    final group = _getGroupForQuestion(q);
    int answeredCount = _answers.values.where((ans) => ans != null && ans.isNotEmpty).length;

    return PopScope(
      canPop: false,
      onPopInvokedWithResult: (didPop, result) async {
        if (didPop) return;
        final confirm = await showDialog<bool>(
          context: context,
          builder: (ctx) => AlertDialog(
            shape: RoundedRectangleBorder(borderRadius: BorderRadius.circular(16)),
            title: const Text('Thoát bài thi?'),
            content: const Text('Tiến trình làm bài hiện tại sẽ không được lưu. Bạn có chắc chắn muốn thoát không?'),
            actions: [
              TextButton(onPressed: () => Navigator.of(ctx).pop(false), child: const Text('Ở lại')),
              ElevatedButton(
                onPressed: () => Navigator.of(ctx).pop(true),
                style: ElevatedButton.styleFrom(backgroundColor: AppColors.redError),
                child: const Text('Thoát'),
              ),
            ],
          ),
        );
        if (confirm == true && context.mounted) {
          Navigator.of(context).pop();
        }
      },
      child: Scaffold(
        backgroundColor: AppColors.background,
        appBar: _buildAppBar(),
        body: Column(
          children: [
            _buildProgressBar(answeredCount),
            Expanded(
              child: SingleChildScrollView(
                padding: const EdgeInsets.fromLTRB(16, 12, 16, 20),
                child: Column(
                  crossAxisAlignment: CrossAxisAlignment.start,
                  children: [
                    // Reading Passage Card if Part 6 or Part 7
                    if (group != null) _buildPassageCard(group),
                    const SizedBox(height: 12),
                    // Question Card
                    _buildQuestionCard(q),
                  ],
                ),
              ),
            ),
            _buildBottomBar(),
          ],
        ),
      ),
    );
  }

  PreferredSizeWidget _buildAppBar() {
    int minutes = _remainingSeconds ~/ 60;
    int seconds = _remainingSeconds % 60;
    String timerText = '${minutes.toString().padLeft(2, '0')}:${seconds.toString().padLeft(2, '0')}';
    bool isUrgent = _remainingSeconds < 300; // < 5 mins

    return AppBar(
      backgroundColor: AppColors.primaryRoyalBlue,
      leading: IconButton(
        icon: const Icon(Icons.arrow_back_ios_new_rounded, size: 20),
        onPressed: () => Navigator.maybePop(context),
      ),
      title: Text(
        widget.examTitle,
        maxLines: 1,
        overflow: TextOverflow.ellipsis,
        style: const TextStyle(fontSize: 16, fontWeight: FontWeight.bold),
      ),
      actions: [
        // Countdown Timer Widget (Tap to demo 10s fast forward)
        GestureDetector(
          onTap: _fastForwardTimerDemo,
          child: Container(
            margin: const EdgeInsets.symmetric(vertical: 10),
            padding: const EdgeInsets.symmetric(horizontal: 10, vertical: 4),
            decoration: BoxDecoration(
              color: isUrgent ? AppColors.redError : Colors.white.withValues(alpha: 0.2),
              borderRadius: BorderRadius.circular(16),
              border: Border.all(
                color: isUrgent ? Colors.white : Colors.transparent,
                width: 1.5,
              ),
            ),
            child: Row(
              children: [
                Icon(
                  Icons.alarm,
                  size: 16,
                  color: isUrgent ? Colors.white : const Color(0xFFFDE68A),
                ),
                const SizedBox(width: 4),
                Text(
                  timerText,
                  style: TextStyle(
                    fontSize: 14,
                    fontWeight: FontWeight.bold,
                    color: isUrgent ? Colors.white : Colors.white,
                    fontFeatures: const [FontFeature.tabularFigures()],
                  ),
                ),
              ],
            ),
          ),
        ),
        const SizedBox(width: 8),
        // Submit Button
        Padding(
          padding: const EdgeInsets.only(right: 12),
          child: ElevatedButton(
            onPressed: _confirmSubmit,
            style: ElevatedButton.styleFrom(
              backgroundColor: AppColors.emeraldGreen,
              foregroundColor: Colors.white,
              padding: const EdgeInsets.symmetric(horizontal: 14, vertical: 8),
              shape: RoundedRectangleBorder(borderRadius: BorderRadius.circular(10)),
              elevation: 0,
            ),
            child: const Text('Submit', style: TextStyle(fontSize: 13, fontWeight: FontWeight.bold)),
          ),
        ),
      ],
    );
  }

  Widget _buildProgressBar(int answeredCount) {
    double progress = _questions.isEmpty ? 0 : answeredCount / _questions.length;
    final currentQ = _questions[_currentIndex];
    int currentPart = currentQ.questionNumber <= 130 ? 5 : currentQ.questionNumber <= 146 ? 6 : 7;

    return Container(
      padding: const EdgeInsets.symmetric(horizontal: 16, vertical: 10),
      color: Colors.white,
      child: Column(
        children: [
          Row(
            mainAxisAlignment: MainAxisAlignment.spaceBetween,
            children: [
              // Quick Part Switcher
              Row(
                children: [
                  _buildPartTab(5, 'Part 5', currentPart == 5),
                  const SizedBox(width: 6),
                  _buildPartTab(6, 'Part 6', currentPart == 6),
                  const SizedBox(width: 6),
                  _buildPartTab(7, 'Part 7', currentPart == 7),
                ],
              ),
              // Answered indicator
              Text(
                '$answeredCount / ${_questions.length} answered',
                style: const TextStyle(
                  fontSize: 12,
                  fontWeight: FontWeight.w600,
                  color: AppColors.textMuted,
                ),
              ),
            ],
          ),
          const SizedBox(height: 8),
          ClipRRect(
            borderRadius: BorderRadius.circular(4),
            child: LinearProgressIndicator(
              value: progress,
              minHeight: 5,
              backgroundColor: AppColors.surfaceMuted,
              valueColor: const AlwaysStoppedAnimation<Color>(AppColors.emeraldGreen),
            ),
          ),
        ],
      ),
    );
  }

  Widget _buildPartTab(int partNum, String title, bool isActive) {
    return GestureDetector(
      onTap: () => _jumpToPart(partNum),
      child: Container(
        padding: const EdgeInsets.symmetric(horizontal: 10, vertical: 4),
        decoration: BoxDecoration(
          color: isActive ? AppColors.primaryRoyalBlue : AppColors.surfaceMuted,
          borderRadius: BorderRadius.circular(8),
        ),
        child: Text(
          title,
          style: TextStyle(
            fontSize: 11,
            fontWeight: FontWeight.bold,
            color: isActive ? Colors.white : AppColors.textMuted,
          ),
        ),
      ),
    );
  }

  Widget _buildPassageCard(QuestionGroupItem group) {
    return Container(
      width: double.infinity,
      padding: const EdgeInsets.all(16),
      decoration: BoxDecoration(
        color: Colors.white,
        borderRadius: BorderRadius.circular(16),
        border: Border.all(color: const Color(0xFFBAE6FD)),
        boxShadow: [
          BoxShadow(
            color: Colors.black.withValues(alpha: 0.02),
            blurRadius: 8,
            offset: const Offset(0, 2),
          ),
        ],
      ),
      child: Column(
        crossAxisAlignment: CrossAxisAlignment.start,
        children: [
          Row(
            children: [
              Container(
                padding: const EdgeInsets.symmetric(horizontal: 8, vertical: 4),
                decoration: BoxDecoration(
                  color: const Color(0xFFE0F2FE),
                  borderRadius: BorderRadius.circular(6),
                ),
                child: Row(
                  children: [
                    const Icon(Icons.article_outlined, size: 14, color: Color(0xFF0284C7)),
                    const SizedBox(width: 4),
                    Text(
                      group.groupTitle,
                      style: const TextStyle(
                        fontSize: 12,
                        fontWeight: FontWeight.bold,
                        color: Color(0xFF0369A1),
                      ),
                    ),
                  ],
                ),
              ),
            ],
          ),
          const SizedBox(height: 12),
          Text(
            group.passageText,
            style: const TextStyle(
              fontSize: 13.5,
              height: 1.55,
              color: AppColors.textDark,
            ),
          ),
          if (group.imageUrl != null && group.imageUrl!.isNotEmpty) ...[
            const SizedBox(height: 12),
            ClipRRect(
              borderRadius: BorderRadius.circular(10),
              child: Image.network(
                group.imageUrl!.startsWith('http')
                    ? group.imageUrl!
                    : '${ApiService().baseUrl}${group.imageUrl}',
                errorBuilder: (ctx, err, stack) => Container(
                  padding: const EdgeInsets.all(12),
                  color: AppColors.surfaceMuted,
                  child: const Row(
                    mainAxisAlignment: MainAxisAlignment.center,
                    children: [
                      Icon(Icons.image_outlined, color: AppColors.textMuted),
                      SizedBox(width: 6),
                      Text('Hình ảnh minh họa bài đọc', style: TextStyle(fontSize: 12, color: AppColors.textMuted)),
                    ],
                  ),
                ),
              ),
            ),
          ],
        ],
      ),
    );
  }

  Widget _buildQuestionCard(QuestionItem q) {
    final selectedOption = _answers[q.id];

    return Container(
      width: double.infinity,
      padding: const EdgeInsets.all(18),
      decoration: BoxDecoration(
        color: Colors.white,
        borderRadius: BorderRadius.circular(18),
        border: Border.all(color: AppColors.border),
        boxShadow: [
          BoxShadow(
            color: Colors.black.withValues(alpha: 0.03),
            blurRadius: 10,
            offset: const Offset(0, 4),
          ),
        ],
      ),
      child: Column(
        crossAxisAlignment: CrossAxisAlignment.start,
        children: [
          Row(
            mainAxisAlignment: MainAxisAlignment.spaceBetween,
            children: [
              Container(
                padding: const EdgeInsets.symmetric(horizontal: 10, vertical: 5),
                decoration: BoxDecoration(
                  color: AppColors.royalBlueSubtle,
                  borderRadius: BorderRadius.circular(8),
                ),
                child: Text(
                  'Question ${q.questionNumber}',
                  style: const TextStyle(
                    fontSize: 13,
                    fontWeight: FontWeight.bold,
                    color: AppColors.primaryRoyalBlue,
                  ),
                ),
              ),
              if (selectedOption != null)
                Container(
                  padding: const EdgeInsets.symmetric(horizontal: 8, vertical: 3),
                  decoration: BoxDecoration(
                    color: AppColors.emeraldLight,
                    borderRadius: BorderRadius.circular(6),
                  ),
                  child: const Row(
                    children: [
                      Icon(Icons.check, size: 12, color: AppColors.emeraldDark),
                      SizedBox(width: 4),
                      Text(
                        'Answer saved',
                        style: TextStyle(
                          fontSize: 11,
                          fontWeight: FontWeight.w600,
                          color: AppColors.emeraldDark,
                        ),
                      ),
                    ],
                  ),
                ),
            ],
          ),
          if (q.questionText != null && q.questionText!.isNotEmpty) ...[
            const SizedBox(height: 12),
            Text(
              q.questionText!,
              style: const TextStyle(
                fontSize: 15,
                fontWeight: FontWeight.w600,
                color: AppColors.textDark,
                height: 1.4,
              ),
            ),
          ],
          const SizedBox(height: 16),
          // 4 Options
          _buildOptionTile('A', q.optionA, selectedOption == 'A'),
          _buildOptionTile('B', q.optionB, selectedOption == 'B'),
          _buildOptionTile('C', q.optionC, selectedOption == 'C'),
          _buildOptionTile('D', q.optionD, selectedOption == 'D'),
        ],
      ),
    );
  }

  Widget _buildOptionTile(String optionKey, String optionText, bool isSelected) {
    return GestureDetector(
      onTap: () => _selectOption(optionKey),
      child: AnimatedContainer(
        duration: const Duration(milliseconds: 150),
        margin: const EdgeInsets.only(bottom: 10),
        padding: const EdgeInsets.symmetric(horizontal: 14, vertical: 12),
        decoration: BoxDecoration(
          color: isSelected ? AppColors.royalBlueSubtle : Colors.white,
          borderRadius: BorderRadius.circular(12),
          border: Border.all(
            color: isSelected ? AppColors.primaryRoyalBlue : AppColors.border,
            width: isSelected ? 2 : 1,
          ),
        ),
        child: Row(
          children: [
            Container(
              width: 28,
              height: 28,
              decoration: BoxDecoration(
                color: isSelected ? AppColors.primaryRoyalBlue : AppColors.surfaceMuted,
                shape: BoxShape.circle,
              ),
              alignment: Alignment.center,
              child: Text(
                optionKey,
                style: TextStyle(
                  fontSize: 13,
                  fontWeight: FontWeight.bold,
                  color: isSelected ? Colors.white : AppColors.textDark,
                ),
              ),
            ),
            const SizedBox(width: 12),
            Expanded(
              child: Text(
                optionText,
                style: TextStyle(
                  fontSize: 14,
                  fontWeight: isSelected ? FontWeight.w600 : FontWeight.normal,
                  color: isSelected ? AppColors.primaryRoyalBlue : AppColors.textDark,
                ),
              ),
            ),
            if (isSelected)
              const Icon(Icons.check_circle, color: AppColors.primaryRoyalBlue, size: 20),
          ],
        ),
      ),
    );
  }

  Widget _buildBottomBar() {
    bool hasPrevious = _currentIndex > 0;
    bool hasNext = _currentIndex < _questions.length - 1;

    return Container(
      padding: const EdgeInsets.symmetric(horizontal: 16, vertical: 12),
      decoration: const BoxDecoration(
        color: Colors.white,
        border: Border(top: BorderSide(color: AppColors.border)),
      ),
      child: SafeArea(
        top: false,
        child: Row(
          children: [
            // Previous button
            IconButton(
              icon: const Icon(Icons.arrow_back_rounded),
              onPressed: hasPrevious
                  ? () => setState(() => _currentIndex--)
                  : null,
              color: AppColors.primaryRoyalBlue,
              tooltip: 'Previous Question',
            ),
            // Clear Selection button
            TextButton.icon(
              onPressed: _clearCurrentAnswer,
              icon: const Icon(Icons.clear_rounded, size: 16, color: AppColors.textMuted),
              label: const Text('Clear', style: TextStyle(fontSize: 12, color: AppColors.textMuted)),
            ),
            const Spacer(),
            // Question Grid Button
            OutlinedButton.icon(
              onPressed: _showQuestionGrid,
              icon: const Icon(Icons.grid_view_rounded, size: 16),
              label: const Text('Question Grid', style: TextStyle(fontSize: 12)),
              style: OutlinedButton.styleFrom(
                foregroundColor: AppColors.primaryRoyalBlue,
                side: const BorderSide(color: AppColors.primaryRoyalBlue),
                shape: RoundedRectangleBorder(borderRadius: BorderRadius.circular(10)),
                padding: const EdgeInsets.symmetric(horizontal: 12, vertical: 8),
              ),
            ),
            const SizedBox(width: 8),
            // Next button
            ElevatedButton(
              onPressed: hasNext
                  ? () => setState(() => _currentIndex++)
                  : _confirmSubmit,
              style: ElevatedButton.styleFrom(
                backgroundColor: hasNext ? AppColors.primaryRoyalBlue : AppColors.emeraldGreen,
                padding: const EdgeInsets.symmetric(horizontal: 16, vertical: 10),
                shape: RoundedRectangleBorder(borderRadius: BorderRadius.circular(10)),
              ),
              child: Row(
                children: [
                  Text(hasNext ? 'Next' : 'Finish', style: const TextStyle(fontSize: 13, fontWeight: FontWeight.bold)),
                  const SizedBox(width: 4),
                  Icon(hasNext ? Icons.arrow_forward_rounded : Icons.check_rounded, size: 16),
                ],
              ),
            ),
          ],
        ),
      ),
    );
  }
}
