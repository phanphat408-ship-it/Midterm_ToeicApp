using System;
using System.IO;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;

namespace ToeicReadingApp.Exams.WordParser
{
    public static class WordExamSampleGenerator
    {
        public static byte[] GenerateValidSampleDocx(string examTitle = "TOEIC Reading Practice Test 05")
        {
            using (var mem = new MemoryStream())
            {
                using (var wordDoc = WordprocessingDocument.Create(mem, WordprocessingDocumentType.Document, true))
                {
                    var mainPart = wordDoc.AddMainDocumentPart();
                    mainPart.Document = new Document();
                    var body = mainPart.Document.AppendChild(new Body());

                    AddPara(body, $"[EXAM_TITLE] {examTitle}");
                    AddPara(body, "[DURATION] 75");

                    // =========================================================================
                    // PART 5: QUESTIONS 101 - 130 (INCOMPLETE SENTENCES)
                    // =========================================================================
                    AddPara(body, "[PART:5]");
                    AddPara(body, "[DIRECTIONS] In each question, you will find a word or phrase missing. Four answer choices are given. Choose the one that best completes the sentence.");

                    AddQuestion(body, 101, "Customer satisfaction is our highest priority at Kingston Financial Services.", "completely", "total", "absolute", "entire", "B");
                    AddQuestion(body, 102, "Ms. Delgado will present the quarterly budget report ------- the board members tomorrow morning.", "to", "at", "for", "with", "A");
                    AddQuestion(body, 103, "All employees must submit their travel expense receipts within thirty days ------- their return.", "following", "before", "while", "during", "A");
                    AddQuestion(body, 104, "The newly designed electric automobile operates much more ------- than earlier models.", "efficient", "efficiency", "efficiently", "more efficient", "C");
                    AddQuestion(body, 105, "Conference attendees are requested to wear their name tags at ------- times inside the convention center.", "each", "all", "every", "whole", "B");
                    AddQuestion(body, 106, "Mr. Tanaka asked his assistant to arrange a meeting ------- the marketing directors of both firms.", "among", "between", "through", "beside", "B");
                    AddQuestion(body, 107, "The warranty covers parts and labor for any defect that occurs under ------- operating conditions.", "normal", "normally", "normalize", "normality", "A");
                    AddQuestion(body, 108, "Because of severe winter weather, tomorrow's flight to Toronto has been ------- canceled.", "temporarily", "temporary", "temperance", "temperament", "A");
                    AddQuestion(body, 109, "Please review the attached contract carefully and return the signed copy ------- Friday afternoon.", "until", "by", "since", "from", "B");
                    AddQuestion(body, 110, "The human resources division has announced an opening for an ------- experienced graphic designer.", "extremely", "extreme", "extremity", "extremes", "A");
                    AddQuestion(body, 111, "Visitors to the laboratory are required to be ------- by an authorized staff member at all times.", "accompanied", "accompanying", "accompaniment", "accompany", "A");
                    AddQuestion(body, 112, "The catering company provided a variety of vegetarian choices ------- meat dishes for the banquet.", "as well as", "in addition", "likewise", "together", "A");
                    AddQuestion(body, 113, "Sales of the innovative solar-powered charger have risen ------- since its launch last quarter.", "dramatically", "dramatic", "drama", "dramatize", "A");
                    AddQuestion(body, 114, "If you experience any difficulties accessing the network, please contact technical support -------.", "immediately", "immediate", "immediacy", "most immediate", "A");
                    AddQuestion(body, 115, "Mr. Gomez is known for his ability to solve complex logistics problems -------.", "creatively", "creation", "creative", "creativeness", "A");
                    AddQuestion(body, 116, "The proposed factory expansion will create more than two hundred jobs in the ------- community.", "local", "locally", "locality", "localize", "A");
                    AddQuestion(body, 117, "Neither Ms. Larson ------- her team members were aware of the revised shipping guidelines.", "nor", "or", "and", "but", "A");
                    AddQuestion(body, 118, "The renovated cafeteria will officially reopen on Monday, offering a wider ------- of healthy meals.", "selection", "select", "selective", "selectively", "A");
                    AddQuestion(body, 119, "Passengers should remain seated with their seatbelts fastened ------- the aircraft comes to a complete stop.", "until", "during", "while", "along", "A");
                    AddQuestion(body, 120, "The newly appointed CEO has proposed a bold strategy to expand into ------- European markets.", "emerging", "emerge", "emergence", "emerged", "A");
                    AddQuestion(body, 121, "Tickets for the international symphony performance are available online and at the box office -------.", "window", "display", "desk", "counter", "A");
                    AddQuestion(body, 122, "Employees who complete twenty years of dedicated service receive a gold watch as a token of -------.", "appreciation", "appreciate", "appreciative", "appreciatively", "A");
                    AddQuestion(body, 123, "The city council voted unanimously to allocate additional funding for public transportation -------.", "improvements", "improved", "improving", "improvisation", "A");
                    AddQuestion(body, 124, "Please ensure that all confidential documents are placed in the secure shredding bin before ------- the office.", "leaving", "left", "leaves", "leave", "A");
                    AddQuestion(body, 125, "Although the weather was unfavorable, the outdoor corporate charity marathon proceeded as -------.", "scheduled", "scheduling", "schedule", "schedules", "A");
                    AddQuestion(body, 126, "The quality assurance inspector examined each pharmaceutical batch ------- before issuing approval.", "thoroughly", "thorough", "thoroughness", "more thorough", "A");
                    AddQuestion(body, 127, "Customers who enroll in the loyalty program earn reward points on every qualifying -------.", "purchase", "purchasing", "purchased", "purchaser", "A");
                    AddQuestion(body, 128, "The board decided to postpone the merger announcement ------- further regulatory review is finalized.", "until", "despite", "whereas", "in spite of", "A");
                    AddQuestion(body, 129, "Our legal department concluded that the draft agreement complies with all applicable international -------.", "regulations", "regulate", "regulatory", "regulators", "A");
                    AddQuestion(body, 130, "The keynote speaker delivered an inspiring address that received an enthusiastic ------- from the audience.", "reaction", "react", "reactive", "reacting", "A");

                    // =========================================================================
                    // PART 6: QUESTIONS 131 - 146 (TEXT COMPLETION - 4 GROUPS)
                    // =========================================================================
                    AddPara(body, "[PART:6]");
                    AddPara(body, "[DIRECTIONS] Read the texts that follow. A word, phrase, or sentence is missing in parts of each text. Four answer choices for each question are given on below. Select the best answer.");

                    // Group 1: Questions 131-134 (Email)
                    AddPara(body, "[GROUP_START]");
                    AddPara(body, "[GROUP_TITLE] Questions 131-134 refer to the following email.");
                    AddPara(body, "[PASSAGE_START]");
                    AddPara(body, "To: All Staff Members <staff@apexsolutions.com>");
                    AddPara(body, "From: Operations Department <operations@apexsolutions.com>");
                    AddPara(body, "Date: October 15");
                    AddPara(body, "Subject: System Maintenance Notice");
                    AddPara(body, "Please be advised that the corporate intranet server will undergo routine maintenance this Saturday, starting at 10:00 PM. [131] the upgrade, email and file sharing services will be temporarily unavailable. We expect all systems to be fully [132] by Sunday at 6:00 AM.");
                    AddPara(body, "We apologize for any inconvenience this temporary disruption may cause. [133]. Thank you for your cooperation and [134] during this essential update.");
                    AddPara(body, "[PASSAGE_END]");
                    AddQuestion(body, 131, "", "During", "Since", "While", "Until", "A");
                    AddQuestion(body, 132, "", "operating", "operational", "operation", "operated", "B");
                    AddQuestion(body, 133, "", "Please contact the help desk if you experience issues on Monday.", "The office will be closed all weekend.", "Lunch will be provided in the cafeteria.", "Please turn off your monitors before leaving.", "A");
                    AddQuestion(body, 134, "", "patience", "patient", "patiently", "patienceful", "A");
                    AddPara(body, "[GROUP_END]");

                    // Group 2: Questions 135-138 (Notice)
                    AddPara(body, "[GROUP_START]");
                    AddPara(body, "[GROUP_TITLE] Questions 135-138 refer to the following notice.");
                    AddPara(body, "[PASSAGE_START]");
                    AddPara(body, "ATTENTION BUILDING RESIDENTS: ANNUAL FIRE ALARM INSPECTION");
                    AddPara(body, "On Tuesday, November 12, technicians from SafeGuard Fire Services will conduct the annual testing of our fire safety systems. The inspection is scheduled to begin at 9:00 AM and [135] by 3:00 PM.");
                    AddPara(body, "During this period, sirens and strobe lights will activate periodically throughout hallways and inside individual apartments. [136]. You do not need to evacuate unless instructed by our building management.");
                    AddPara(body, "If you have pets that may become frightened by loud alarms, we [137] keeping them in a quiet interior room. We appreciate your [138] as we work to keep our community safe.");
                    AddPara(body, "[PASSAGE_END]");
                    AddQuestion(body, 135, "", "conclude", "concludes", "concluding", "has concluded", "A");
                    AddQuestion(body, 136, "", "These are only routine tests and do not signal an emergency.", "The building elevators will be shut down all week.", "Parking will be restricted in the underground garage.", "All residents must leave the property immediately.", "A");
                    AddQuestion(body, 137, "", "recommend", "recommendation", "recommended", "recommending", "A");
                    AddQuestion(body, 138, "", "understanding", "understand", "understandable", "understandably", "A");
                    AddPara(body, "[GROUP_END]");

                    // Group 3: Questions 139-142 (Memo)
                    AddPara(body, "[GROUP_START]");
                    AddPara(body, "[GROUP_TITLE] Questions 139-142 refer to the following memorandum.");
                    AddPara(body, "[PASSAGE_START]");
                    AddPara(body, "MEMORANDUM");
                    AddPara(body, "To: Department Managers");
                    AddPara(body, "From: Financial Planning Committee");
                    AddPara(body, "Date: December 1");
                    AddPara(body, "Subject: Fiscal Year 2027 Budget Submissions");
                    AddPara(body, "As we approach the end of the current fiscal year, department heads must submit their budget proposals for next year. [139] templates are available for download on the company portal under the Finance tab.");
                    AddPara(body, "Please submit all completed spreadsheets no later than December 20. Late submissions will [140] review delays and may affect department staffing allocations. [141]. Thank you for your prompt attention to this matter, and please ensure your estimates are as [142] as possible.");
                    AddPara(body, "[PASSAGE_END]");
                    AddQuestion(body, 139, "", "Updated", "Update", "Updating", "Updates", "A");
                    AddQuestion(body, 140, "", "cause", "caused", "causing", "causes", "A");
                    AddQuestion(body, 141, "", "Finance officers will hold an informational Q&A session next Tuesday.", "Our headquarters is relocating to Singapore next year.", "Bonuses will be distributed in cash next week.", "Employee ID cards must be renewed before January.", "A");
                    AddQuestion(body, 142, "", "accurate", "accurately", "accuracy", "accurateness", "A");
                    AddPara(body, "[GROUP_END]");

                    // Group 4: Questions 143-146 (Letter)
                    AddPara(body, "[GROUP_START]");
                    AddPara(body, "[GROUP_TITLE] Questions 143-146 refer to the following letter.");
                    AddPara(body, "[PASSAGE_START]");
                    AddPara(body, "Dear Valued Customer,");
                    AddPara(body, "Thank you for subscribing to Horizon Business Weekly for the past five years. Your current subscription is due to [143] on January 31.");
                    AddPara(body, "To ensure uninterrupted access to award-winning reporting, market analysis, and executive interviews, renew today. [144]. When you renew for two years, you will receive our exclusive Global Trend Forecast booklet [145] of charge.");
                    AddPara(body, "Simply complete and return the enclosed card or visit our website to renew in minutes. We look forward to [146] you for another successful year.");
                    AddPara(body, "Sincerely,");
                    AddPara(body, "The Horizon Publishing Team");
                    AddPara(body, "[PASSAGE_END]");
                    AddQuestion(body, 143, "", "expire", "expiration", "expired", "expiring", "A");
                    AddQuestion(body, 144, "", "As a loyal subscriber, you qualify for a 25% discount off the standard rate.", "Our print magazine has been discontinued permanently.", "Subscriptions must be cancelled thirty days in advance.", "Please send all payments directly to our overseas office.", "A");
                    AddQuestion(body, 145, "", "free", "freedom", "freely", "freest", "A");
                    AddQuestion(body, 146, "", "serving", "serve", "served", "servant", "A");
                    AddPara(body, "[GROUP_END]");

                    // =========================================================================
                    // PART 7: QUESTIONS 147 - 200 (READING COMPREHENSION)
                    // =========================================================================
                    AddPara(body, "[PART:7]");
                    AddPara(body, "[DIRECTIONS] In this part, you will read a selection of texts, such as magazine and newspaper articles, letters, and advertisements. Each text is followed by several questions. Select the best answer for each question.");

                    // Group 1: 147-148 (Advertisement)
                    AddPara(body, "[GROUP_START]");
                    AddPara(body, "[GROUP_TITLE] Questions 147-148 refer to the following advertisement.");
                    AddPara(body, "[PASSAGE_START]");
                    AddPara(body, "SUMMIT EXECUTIVE SUITES: DOWNTOWN OFFICE LEASING");
                    AddPara(body, "Looking for a prestigious business address without long-term commercial commitments? Summit Executive Suites offers fully furnished private offices, co-working desks, and state-of-the-art conference rooms located on floors 22-24 of the landmark Horizon Tower.");
                    AddPara(body, "All rental packages include high-speed fiber internet, receptionist services, mail handling, and complimentary beverage stations. Flexible month-to-month leases available.");
                    AddPara(body, "Contact leasing agent Clara Barton at cbarton@summitsuites.com to arrange a guided tour today.");
                    AddPara(body, "[PASSAGE_END]");
                    AddQuestion(body, 147, "What is being offered for lease?", "Retail store spaces", "Furnished office facilities", "Apartment penthouses", "Industrial warehouses", "B");
                    AddQuestion(body, 148, "What feature is mentioned about the lease terms?", "They require a five-year contract.", "They are available on a month-to-month basis.", "They include free employee parking.", "They must be paid in foreign currency.", "B");
                    AddPara(body, "[GROUP_END]");

                    // Group 2: 149-150 (Email)
                    AddPara(body, "[GROUP_START]");
                    AddPara(body, "[GROUP_TITLE] Questions 149-150 refer to the following email.");
                    AddPara(body, "[PASSAGE_START]");
                    AddPara(body, "To: Kenji Sato <ksato@satologistics.jp>");
                    AddPara(body, "From: Maria Lindqvist <mlindqvist@nordicfreight.se>");
                    AddPara(body, "Date: March 8");
                    AddPara(body, "Subject: Cargo Tracking - Shipment #NF-8821");
                    AddPara(body, "Dear Mr. Sato,");
                    AddPara(body, "I am writing to update you on shipment #NF-8821 containing precision automotive components. Due to heavy fog in Rotterdam harbor yesterday, the cargo vessel Scandinavian Trader was delayed by twelve hours.");
                    AddPara(body, "The ship has now docked and customs clearance is underway. The consignment will be transferred to our express rail carrier tomorrow morning and is expected to arrive at your Yokohama warehouse by March 14, two days ahead of your manufacturing deadline.");
                    AddPara(body, "Best regards,");
                    AddPara(body, "Maria Lindqvist, Freight Coordinator");
                    AddPara(body, "[PASSAGE_END]");
                    AddQuestion(body, 149, "Why was the shipment initially delayed?", "Rail tracks were closed for repairs.", "Poor weather conditions in Rotterdam.", "Customs documentation was missing.", "Dockworkers held an unscheduled strike.", "B");
                    AddQuestion(body, 150, "When is the shipment expected to arrive in Yokohama?", "March 8", "March 12", "March 14", "March 16", "C");
                    AddPara(body, "[GROUP_END]");

                    // Group 3: 151-153 (Schedule/Notice)
                    AddPara(body, "[GROUP_START]");
                    AddPara(body, "[GROUP_TITLE] Questions 151-153 refer to the following schedule.");
                    AddPara(body, "[PASSAGE_START]");
                    AddPara(body, "GREENWOOD TECH SYMPOSIUM 2026 - WORKSHOP SCHEDULE (ROOM B)");
                    AddPara(body, "09:00 - 10:15 | Generative AI in Supply Chain Optimization (Dr. Aris Thorne)");
                    AddPara(body, "10:30 - 11:45 | Cyber Defense Best Practices for Remote Teams (Elena Rostova)");
                    AddPara(body, "12:00 - 13:30 | Networking Lunch (Main Terrace - Sponsored by CloudScale)");
                    AddPara(body, "13:45 - 15:00 | Modern Cloud Architecture and Microservices (James O'Connor)");
                    AddPara(body, "15:15 - 16:30 | Quantum Computing: From Theory to Enterprise Deployment (Dr. Mei Lin)");
                    AddPara(body, "Note: Workshop seating is strictly limited to 60 participants per session on a first-come, first-served basis. Attendees must badge in at the entrance.");
                    AddPara(body, "[PASSAGE_END]");
                    AddQuestion(body, 151, "Which workshop begins immediately after the networking lunch?", "Generative AI in Supply Chain", "Modern Cloud Architecture and Microservices", "Cyber Defense Best Practices", "Quantum Computing", "B");
                    AddQuestion(body, 152, "Who is the speaker for the Cyber Defense workshop?", "Dr. Aris Thorne", "Elena Rostova", "James O'Connor", "Dr. Mei Lin", "B");
                    AddQuestion(body, 153, "How are workshop seats allocated?", "By pre-registration online only", "On a first-come, first-served basis", "By lottery ticket number", "According to company size", "B");
                    AddPara(body, "[GROUP_END]");

                    // Group 4: 154-157 (Article)
                    AddPara(body, "[GROUP_START]");
                    AddPara(body, "[GROUP_TITLE] Questions 154-157 refer to the following article.");
                    AddPara(body, "[PASSAGE_START]");
                    AddPara(body, "SOLARIS ENERGY ACQUIRES WINDSTREAM POWER IN $1.2B DEAL");
                    AddPara(body, "HOUSTON (Reuters) - Clean power giant Solaris Energy confirmed on Wednesday that it has completed its acquisition of WindStream Power, the country's third-largest offshore wind operator, in an all-cash transaction valued at $1.2 billion.");
                    AddPara(body, "The deal adds over 4,500 megawatts of generation capacity to Solaris's portfolio across the Atlantic coast. Solaris CEO David Sterling stated that the acquisition accelerates the company's decarbonization goals by a full five years.");
                    AddPara(body, "WindStream's current headquarters in Boston will become Solaris's Northeast Regional Innovation Center. WindStream CEO Rebecca Vance will join the Solaris executive board as Chief Sustainability Officer. Shares of Solaris climbed 4.2% following the announcement.");
                    AddPara(body, "[PASSAGE_END]");
                    AddQuestion(body, 154, "What is the primary subject of the article?", "The bankruptcy of an energy firm", "A corporate corporate buyout between energy companies", "New environmental regulations for offshore wind", "An increase in electricity retail prices", "B");
                    AddQuestion(body, 155, "What will happen to WindStream's Boston headquarters?", "It will be sold to a real estate developer.", "It will become an innovation center.", "It will close permanently by December.", "It will be converted into a manufacturing plant.", "B");
                    AddQuestion(body, 156, "What role will Rebecca Vance assume at Solaris Energy?", "Chief Executive Officer", "Chief Sustainability Officer", "Director of Human Resources", "Head of Financial Operations", "B");
                    AddQuestion(body, 157, "How did financial markets react to the announcement?", "WindStream shares plummeted.", "Solaris shares rose by 4.2%.", "Solaris shares remained unchanged.", "Trading of energy stocks was halted.", "B");
                    AddPara(body, "[GROUP_END]");

                    // Group 5: 158-161 (Product Review / Online Chat)
                    AddPara(body, "[GROUP_START]");
                    AddPara(body, "[GROUP_TITLE] Questions 158-161 refer to the following online discussion.");
                    AddPara(body, "[PASSAGE_START]");
                    AddPara(body, "[10:14 AM] Liam Chen: Has anyone tried the new ergonomics standing desk from ErgoPro?");
                    AddPara(body, "[10:16 AM] Sophia Morales: Yes! We installed twenty of them in the engineering wing last month. The motor is whisper-quiet and the memory preset heights are fantastic.");
                    AddPara(body, "[10:18 AM] Liam Chen: Did assembly take long? We have a small facility team.");
                    AddPara(body, "[10:19 AM] Sophia Morales: Not at all. It took about 25 minutes per desk. All necessary hex keys and hardware were included in the packaging.");
                    AddPara(body, "[10:21 AM] Marcus Brody: Just make sure you order the cable management tray separately. It does not come standard with the basic frame.");
                    AddPara(body, "[10:23 AM] Liam Chen: Good catch, Marcus! I will add those to our purchase order right now.");
                    AddPara(body, "[PASSAGE_END]");
                    AddQuestion(body, 158, "What product are the colleagues discussing?", "Office chairs", "Standing desks", "Computer monitors", "Laptop stands", "B");
                    AddQuestion(body, 159, "What positive feature does Sophia mention about the product?", "The bright color options", "The quiet motor and height presets", "The low shipping cost", "The lifetime replacement warranty", "B");
                    AddQuestion(body, 160, "How long did it take to assemble one desk?", "About 10 minutes", "About 25 minutes", "Over an hour", "A full working day", "B");
                    AddQuestion(body, 161, "What does Marcus advise Liam to purchase separately?", "A wireless mouse pad", "A cable management tray", "An extended warranty", "Castor wheels for the desk", "B");
                    AddPara(body, "[GROUP_END]");

                    // Group 6: 162-165 (Press Release)
                    AddPara(body, "[GROUP_START]");
                    AddPara(body, "[GROUP_TITLE] Questions 162-165 refer to the following press release.");
                    AddPara(body, "[PASSAGE_START]");
                    AddPara(body, "FOR IMMEDIATE RELEASE: KINETIC AIR EXPANDS PACIFIC ROUTES");
                    AddPara(body, "SEATTLE - Low-cost international carrier Kinetic Air today announced daily nonstop service connecting Seattle-Tacoma International Airport (SEA) with Tokyo-Narita (NRT) and Seoul-Incheon (ICN), beginning June 1.");
                    AddPara(body, "Operating state-of-the-art Boeing 787-9 Dreamliner aircraft, flights will offer three cabin classes: Premium Business, Economy Plus, and Standard Economy. Introductory one-way fares will start at just $399 including all taxes and fees for bookings made before April 30.");
                    AddPara(body, "Passengers will enjoy complimentary high-speed Wi-Fi, noise-canceling headphones, and chef-curated meals featuring organic ingredients. Flight tickets are now on sale at www.kineticair.com.");
                    AddPara(body, "[PASSAGE_END]");
                    AddQuestion(body, 162, "When will Kinetic Air commence its new Pacific routes?", "April 30", "June 1", "September 15", "December 1", "B");
                    AddQuestion(body, 163, "Which aircraft model will be used on these flights?", "Boeing 737 Max", "Boeing 787-9 Dreamliner", "Airbus A320neo", "Airbus A380", "B");
                    AddQuestion(body, 164, "How can travelers qualify for the $399 introductory fare?", "By flying only on weekdays", "By booking before April 30", "By traveling without checked baggage", "By signing up for a credit card", "B");
                    AddQuestion(body, 165, "What amenity is provided free of charge to all passengers?", "First-class lounge access", "High-speed Wi-Fi", "Unlimited baggage allowance", "Chauffeur airport transfer", "B");
                    AddPara(body, "[GROUP_END]");

                    // Group 7: 166-170 (Double Passage: Webpage & Email)
                    AddPara(body, "[GROUP_START]");
                    AddPara(body, "[GROUP_TITLE] Questions 166-170 refer to the following webpage and email.");
                    AddPara(body, "[PASSAGE_START]");
                    AddPara(body, "--- DOCUMENT 1: PACIFIC VIEW HOTEL CATERING PACKAGES ---");
                    AddPara(body, "Package A: Continental Breakfast ($28/person) - Croissants, fresh fruit, Greek yogurt, gourmet coffee & tea.");
                    AddPara(body, "Package B: Executive Deli Lunch ($42/person) - Artisanal sandwich platters, mixed garden salad, soup of the day, cookies.");
                    AddPara(body, "Package C: Pacific Buffet Banquet ($65/person) - Grilled salmon, roast tenderloin, seasonal roasted vegetables, cheesecake bar.");
                    AddPara(body, "Audio/Visual Add-On: 4K Projector & Wireless Mic System ($250 flat fee).");
                    AddPara(body, "All catering reservations for groups exceeding 30 persons require a 20% non-refundable deposit upon confirmation.");
                    AddPara(body, "");
                    AddPara(body, "--- DOCUMENT 2: EMAIL ---");
                    AddPara(body, "To: Catering Sales <catering@pacificviewhotel.com>");
                    AddPara(body, "From: Derek Vance <dvance@novasystems.com>");
                    AddPara(body, "Date: July 14");
                    AddPara(body, "Subject: Reservation for Nova Systems Annual Seminar");
                    AddPara(body, "Dear Sales Team,");
                    AddPara(body, "We would like to book the Ocean Ballroom for our annual staff seminar on August 22. We anticipate 40 attendees. We would like to select Package B for our midday meal and we will definitely require the Audio/Visual projector and microphone setup for our presentations.");
                    AddPara(body, "Please send us the invoice for the deposit so our accounts payable team can process the bank transfer immediately.");
                    AddPara(body, "Sincerely, Derek Vance, Office Manager");
                    AddPara(body, "[PASSAGE_END]");
                    AddQuestion(body, 166, "What catering package did Mr. Vance choose?", "Package A", "Package B", "Package C", "Custom menu", "B");
                    AddQuestion(body, 167, "How many attendees does Mr. Vance expect?", "20", "30", "40", "50", "C");
                    AddQuestion(body, 168, "What additional equipment did Nova Systems request?", "Dance floor lighting", "Audio/Visual projector & microphone system", "Mobile photo booth", "Simultaneous translation headsets", "B");
                    AddQuestion(body, 169, "What is the per-person cost of Package B?", "$28", "$42", "$65", "$250", "B");
                    AddQuestion(body, 170, "Why does Mr. Vance request an invoice?", "To cancel his booking", "To pay the required 20% deposit", "To apply for a corporate tax exemption", "To upgrade to Ocean Ballroom VIP suite", "B");
                    AddPara(body, "[GROUP_END]");

                    // Group 8: 171-175 (Double Passage: Invoice & Review)
                    AddPara(body, "[GROUP_START]");
                    AddPara(body, "[GROUP_TITLE] Questions 171-175 refer to the following invoice and customer review.");
                    AddPara(body, "[PASSAGE_START]");
                    AddPara(body, "--- INVOICE #9042 - APEX OFFICE DIRECT ---");
                    AddPara(body, "Sold to: Belmont Architecture Studio, 104 Waterfront Way");
                    AddPara(body, "Date: September 5");
                    AddPara(body, "Item 1: 5x ProDraft 32-inch 4K Monitors ($450 each) = $2,250");
                    AddPara(body, "Item 2: 5x Ergonomic Monitor Arms ($80 each) = $400");
                    AddPara(body, "Shipping: White Glove Express Delivery = $120");
                    AddPara(body, "Total: $2,770");
                    AddPara(body, "Delivery Note: Signed for by Janice Riley on September 9.");
                    AddPara(body, "");
                    AddPara(body, "--- ONLINE REVIEW BY JANICE RILEY ---");
                    AddPara(body, "Rating: 5/5 Stars");
                    AddPara(body, "Reviewed on September 15: Outstanding clarity and build quality! Our architectural designers are thrilled with the ProDraft 4K displays. Color accuracy is phenomenal for CAD blueprints. Delivery was on time and the courier helped carry the heavy boxes directly into our design studio on the third floor. Highly recommended for creative agencies!");
                    AddPara(body, "[PASSAGE_END]");
                    AddQuestion(body, 171, "What type of business is Belmont Architecture Studio?", "A law firm", "An architecture and design studio", "A furniture retailer", "A computer repair shop", "B");
                    AddQuestion(body, 172, "How many monitors were purchased on Invoice #9042?", "2", "5", "10", "32", "B");
                    AddQuestion(body, 173, "When was the order delivered?", "September 5", "September 9", "September 15", "September 30", "B");
                    AddQuestion(body, 174, "What aspect of the delivery service did Ms. Riley appreciate?", "Free return shipping", "The courier carried boxes to the 3rd floor studio", "Gift wrapping of monitors", "Same-day delivery service", "B");
                    AddQuestion(body, 175, "What rating did Ms. Riley assign to the products?", "2 out of 5", "3 out of 5", "4 out of 5", "5 out of 5", "D");
                    AddPara(body, "[GROUP_END]");

                    // Group 9: 176-180 (Triple Passage: Job Ad, Application Email, Interview Invite)
                    AddPara(body, "[GROUP_START]");
                    AddPara(body, "[GROUP_TITLE] Questions 176-180 refer to the following job advertisement, email, and confirmation.");
                    AddPara(body, "[PASSAGE_START]");
                    AddPara(body, "--- TEXT 1: JOB OPENING: SENIOR LOGISTICS ANALYST ---");
                    AddPara(body, "Company: TransGlobal Logistics Inc., Chicago, IL");
                    AddPara(body, "Requirements: Bachelor's degree in Supply Chain or related field; minimum 5 years international freight forwarding experience; fluency in SAP Transportation Management; bilingual English/Spanish preferred.");
                    AddPara(body, "");
                    AddPara(body, "--- TEXT 2: APPLICATION EMAIL ---");
                    AddPara(body, "To: Careers <recruiting@transglobal.com>");
                    AddPara(body, "From: Carlos Morales <cmorales@chicagomail.com>");
                    AddPara(body, "Date: October 4");
                    AddPara(body, "Subject: Application: Senior Logistics Analyst - Carlos Morales");
                    AddPara(body, "Dear Hiring Committee,");
                    AddPara(body, "I am writing to express my enthusiastic interest in the Senior Logistics Analyst position. I hold a B.S. in Industrial Logistics from Purdue University and have spent the past six years optimizing container freight routes for Pacific Express. I am proficient in SAP and native bilingual in English and Spanish.");
                    AddPara(body, "");
                    AddPara(body, "--- TEXT 3: INTERVIEW INVITATION ---");
                    AddPara(body, "To: Carlos Morales <cmorales@chicagomail.com>");
                    AddPara(body, "From: Sandra Wu, HR Director <swu@transglobal.com>");
                    AddPara(body, "Date: October 8");
                    AddPara(body, "Dear Mr. Morales, We were very impressed by your background. We would like to invite you for a 45-minute video interview on Thursday, October 15 at 2:00 PM CST via Zoom.");
                    AddPara(body, "[PASSAGE_END]");
                    AddQuestion(body, 176, "What position is Mr. Morales applying for?", "Warehouse Supervisor", "Senior Logistics Analyst", "Fleet Dispatcher", "Chief Financial Officer", "B");
                    AddQuestion(body, 177, "How many years of relevant experience does Mr. Morales possess?", "2 years", "4 years", "6 years", "10 years", "C");
                    AddQuestion(body, 178, "What language skills does Mr. Morales highlight?", "English and Mandarin", "English and Spanish", "French and German", "Japanese and Korean", "B");
                    AddQuestion(body, 179, "Who sent the interview invitation to Mr. Morales?", "Carlos Morales", "Sandra Wu", "Aris Thorne", "Janice Riley", "B");
                    AddQuestion(body, 180, "When is the scheduled video interview?", "October 4", "October 8", "October 15", "November 1", "C");
                    AddPara(body, "[GROUP_END]");

                    // Group 10: 181-185 (Triple Passage: Memo, Policy Document, Employee Inquiry)
                    AddPara(body, "[GROUP_START]");
                    AddPara(body, "[GROUP_TITLE] Questions 181-185 refer to the following memo, travel policy, and inquiry email.");
                    AddPara(body, "[PASSAGE_START]");
                    AddPara(body, "--- MEMO: REVISED BUSINESS TRAVEL POLICY ---");
                    AddPara(body, "Effective November 1, all employee domestic flights under four hours must be booked in Economy Class. Hotel room reimbursements are capped at $220/night in major metropolitan centers and $160/night in regional cities.");
                    AddPara(body, "Daily meal per diem is set at $75 per day without the requirement of itemized receipts, provided meals were purchased during active travel days.");
                    AddPara(body, "");
                    AddPara(body, "--- EXPENSE GUIDELINES SUMMARY ---");
                    AddPara(body, "1. Bookings must be made via Corporate Travel Portal at least 14 days in advance.");
                    AddPara(body, "2. Ground transportation: Rideshare services and airport shuttles are preferred over car rentals for trips under three days.");
                    AddPara(body, "");
                    AddPara(body, "--- EMAIL INQUIRY ---");
                    AddPara(body, "To: Travel Helpdesk <travel@vanguard.com>");
                    AddPara(body, "From: Hannah Schmidt <hschmidt@vanguard.com>");
                    AddPara(body, "Date: November 12");
                    AddPara(body, "Subject: San Francisco Client Visit Hotel Budget");
                    AddPara(body, "Hello, I am traveling to San Francisco (major metro center) next week for a 2-day client meeting. The lowest hotel rate near the client's office is $215/night. Since this is within the $220 cap, does this require manager pre-approval? Thanks, Hannah.");
                    AddPara(body, "[PASSAGE_END]");
                    AddQuestion(body, 181, "What is the maximum hotel reimbursement for major metropolitan centers?", "$160/night", "$220/night", "$275/night", "$350/night", "B");
                    AddQuestion(body, 182, "What is the daily meal per diem allowance?", "$45", "$75", "$100", "$150", "B");
                    AddQuestion(body, 183, "How far in advance should business travel be booked?", "At least 3 days", "At least 7 days", "At least 14 days", "At least 30 days", "C");
                    AddQuestion(body, 184, "Why is Ms. Schmidt visiting San Francisco?", "To attend a trade exhibition", "For a 2-day client meeting", "To open a regional branch", "For executive leadership training", "B");
                    AddQuestion(body, 185, "What is the cost of the hotel Ms. Schmidt intends to book?", "$160/night", "$215/night", "$220/night", "$250/night", "B");
                    AddPara(body, "[GROUP_END]");

                    // Group 11: 186-190 (Triple Passage: Newsletter, Product Recall Notice, Customer Service FAQ)
                    AddPara(body, "[GROUP_START]");
                    AddPara(body, "[GROUP_TITLE] Questions 186-190 refer to the following newsletter, recall notice, and FAQ.");
                    AddPara(body, "[PASSAGE_START]");
                    AddPara(body, "--- TECH HORIZONS MONTHLY: SMART KITCHEN APPLIANCES ---");
                    AddPara(body, "Smart appliances continue to transform domestic kitchens. CulinaryTech's AeroBlend 9000 blender recently won the 2026 Home Innovation Gold Award for its ultra-quiet induction motor and Wi-Fi recipe synchronization.");
                    AddPara(body, "");
                    AddPara(body, "--- VOLUNTARY SAFETY RECALL NOTICE: AEROBLEND 9000 ---");
                    AddPara(body, "CulinaryTech Inc. announces a voluntary recall of AeroBlend 9000 blenders manufactured between March 1 and May 15, 2026 (Serial numbers starting with AB9-26M or AB9-26A). Under extreme continuous usage exceeding 15 minutes, the motor gasket may overheat.");
                    AddPara(body, "Remedy: Customers will receive a free upgraded blade assembly and replacement gasket kit by mail.");
                    AddPara(body, "");
                    AddPara(body, "--- FAQ ---");
                    AddPara(body, "Q: How do I check if my blender is affected? Look at the barcode label underneath the blender base.");
                    AddPara(body, "Q: Do I need to send my entire blender back? No, only the blade and gasket are replaced using the provided snap-in kit.");
                    AddPara(body, "[PASSAGE_END]");
                    AddQuestion(body, 186, "What award did the AeroBlend 9000 blender win?", "Best Design Award", "Home Innovation Gold Award", "Consumer Choice Trophy", "Kitchen Appliance of the Century", "B");
                    AddQuestion(body, 187, "Which serial numbers are affected by the voluntary recall?", "AB9-25X only", "AB9-26M and AB9-26A", "AB9-9000Z", "All serial numbers ever produced", "B");
                    AddQuestion(body, 188, "What potential issue prompted the recall?", "The digital touchscreen freezes", "The motor gasket may overheat under extreme continuous use", "The glass pitcher easily cracks", "The power cord is too short", "B");
                    AddQuestion(body, 189, "Where can consumers locate their blender's serial number?", "On the recipe booklet cover", "On the barcode label underneath the base", "On the warranty registration email", "On the power adapter plug", "B");
                    AddQuestion(body, 190, "What is provided to owners of affected blenders?", "A full cash refund", "A free upgraded blade assembly and replacement gasket kit", "A coupon for restaurant delivery", "A brand-new microwave oven", "B");
                    AddPara(body, "[GROUP_END]");

                    // Group 12: 191-195 (Triple Passage: Exhibition Floorplan & Guide, Email, Feedback Form)
                    AddPara(body, "[GROUP_START]");
                    AddPara(body, "[GROUP_TITLE] Questions 191-195 refer to the following exhibition guide, email, and feedback form.");
                    AddPara(body, "[PASSAGE_START]");
                    AddPara(body, "--- INTERNATIONAL AUTO EXPO 2026: EXHIBITION HALL A ---");
                    AddPara(body, "Booth 101-105: EV Motors Corp (Electric sedans & battery technology)");
                    AddPara(body, "Booth 106-110: Aerocool Radiators (Automotive cooling systems)");
                    AddPara(body, "Booth 111-115: Apex Telematics (GPS and connected fleet tracking)");
                    AddPara(body, "Booth 116-120: CyberDrive Sensors (LiDAR and autonomous cameras)");
                    AddPara(body, "");
                    AddPara(body, "--- EMAIL: BOOTH STAFFING SCHEDULE ---");
                    AddPara(body, "To: Apex Telematics Expo Team <expo@apextelematics.com>");
                    AddPara(body, "From: Natalie Cole, Trade Show Coordinator <ncole@apextelematics.com>");
                    AddPara(body, "Date: October 18");
                    AddPara(body, "Team, Our booth (Booths 111-115) will open on Friday at 8:30 AM. Shift 1 (8:30 - 13:00): Kevin, Sam, and Linda. Shift 2 (13:00 - 18:00): Brian, Chloe, and Eric. Please ensure marketing brochures and USB product demo keys are fully restocked on display tables before doors open to the general public.");
                    AddPara(body, "");
                    AddPara(body, "--- VISITOR FEEDBACK FORM ---");
                    AddPara(body, "Visitor: Roger Davenport, Fleet Director, Midwest Freight");
                    AddPara(body, "Comment: Visited Booth 113 on Friday afternoon. Met Brian and Chloe. Their telematics demo on real-time vehicle fuel tracking was very compelling. We have requested a formal sales quote for our fleet of 250 trucks.");
                    AddPara(body, "[PASSAGE_END]");
                    AddQuestion(body, 191, "What does Apex Telematics showcase at the expo?", "Electric passenger sedans", "Automotive cooling radiators", "GPS and connected fleet tracking telematics", "Automotive windshield glass", "C");
                    AddQuestion(body, 192, "Which booth numbers are occupied by Apex Telematics?", "Booths 101-105", "Booths 106-110", "Booths 111-115", "Booths 116-120", "C");
                    AddQuestion(body, 193, "Who was on duty during Shift 2 for Apex Telematics?", "Kevin, Sam, and Linda", "Brian, Chloe, and Eric", "Natalie, Roger, and Kevin", "Sam, Linda, and Chloe", "B");
                    AddQuestion(body, 194, "Who submitted the visitor feedback form?", "Natalie Cole", "Roger Davenport", "Brian Larson", "Chloe Adams", "B");
                    AddQuestion(body, 195, "How many trucks does Midwest Freight operate in its fleet?", "50 trucks", "100 trucks", "250 trucks", "500 trucks", "C");
                    AddPara(body, "[GROUP_END]");

                    // Group 13: 196-200 (Triple Passage: Corporate Announcement, Press Coverage, CEO Letter)
                    AddPara(body, "[GROUP_START]");
                    AddPara(body, "[GROUP_TITLE] Questions 196-200 refer to the following announcement, news article, and letter.");
                    AddPara(body, "[PASSAGE_START]");
                    AddPara(body, "--- CORPORATE ANNOUNCEMENT: NEXUS BIOTECH ---");
                    AddPara(body, "Nexus Biotech announced today that the European Medicines Agency (EMA) has granted marketing authorization for Corvalis, its revolutionary oral treatment for rheumatoid arthritis. Commercial distribution across fifteen EU member states will commence in Q1 2027.");
                    AddPara(body, "");
                    AddPara(body, "--- PHARMA DAILY NEWS REPORT ---");
                    AddPara(body, "GENEVA - Corvalis approval marks a major milestone for Nexus Biotech. Clinical phase 3 trials involving 2,400 patients showed a 78% reduction in joint inflammation symptoms with negligible side effects compared to traditional intravenous biologics. Industry analysts estimate annual European sales will reach 850 million euros within three years.");
                    AddPara(body, "");
                    AddPara(body, "--- LETTER TO SHAREHOLDERS FROM CEO DR. JEAN-LUC MOREAU ---");
                    AddPara(body, "Dear Fellow Shareholders, Following our European regulatory victory with Corvalis, our manufacturing facilities in Dublin and Lyon have already ramped up production to full capacity. We remain firmly on track for FDA filing in the United States by mid-2027.");
                    AddPara(body, "Thank you for your steadfast support as we bring life-changing therapies to millions of patients worldwide.");
                    AddPara(body, "[PASSAGE_END]");
                    AddQuestion(body, 196, "What medication received regulatory marketing authorization?", "Corvalis", "RheumaStop", "BioGenix", "ArthroShield", "A");
                    AddQuestion(body, 197, "Which regulatory body granted marketing authorization?", "US FDA", "European Medicines Agency (EMA)", "Health Canada", "Japanese PMDA", "B");
                    AddQuestion(body, 198, "How many patients participated in the Phase 3 clinical trial?", "500 patients", "1,200 patients", "2,400 patients", "5,000 patients", "C");
                    AddQuestion(body, 199, "Where are Nexus Biotech's manufacturing facilities located?", "Berlin and Madrid", "Dublin and Lyon", "London and Milan", "Geneva and Zurich", "B");
                    AddQuestion(body, 200, "When does Nexus Biotech plan to file with the US FDA?", "Q4 2026", "Mid-2027", "Late 2028", "Early 2030", "B");
                    AddPara(body, "[GROUP_END]");

                    mainPart.Document.Save();
                }

                return mem.ToArray();
            }
        }

        public static byte[] GenerateInvalidSampleDocx()
        {
            using (var mem = new MemoryStream())
            {
                using (var wordDoc = WordprocessingDocument.Create(mem, WordprocessingDocumentType.Document, true))
                {
                    var mainPart = wordDoc.AddMainDocumentPart();
                    mainPart.Document = new Document();
                    var body = mainPart.Document.AppendChild(new Body());

                    AddPara(body, "[EXAM_TITLE] TOEIC Reading Error Test (File Test Bắt Lỗi Định Dạng)");
                    AddPara(body, "[DURATION] 75");

                    AddPara(body, "[PART:5]");
                    AddPara(body, "[DIRECTIONS] Choose the best answer to complete each sentence.");

                    AddQuestion(body, 101, "Customer satisfaction is our highest priority at Kingston Financial Services.", "completely", "total", "absolute", "entire", "B");
                    AddQuestion(body, 102, "Ms. Delgado will present the quarterly budget report to the board tomorrow.", "to", "at", "for", "with", "A");
                    AddQuestion(body, 103, "All employees must submit travel expense receipts within thirty days.", "following", "before", "while", "during", "A");
                    AddQuestion(body, 104, "The newly designed electric automobile operates much more efficiently.", "efficient", "efficiency", "efficiently", "more efficient", "C");

                    // LỖI 1: CÂU 105 CỐ TÌNH THIẾU ĐÁP ÁN [C]!
                    AddPara(body, "[Q:105] Conference attendees are requested to wear their name tags at all times inside the convention center.");
                    AddPara(body, "[A] each");
                    AddPara(body, "[B] all");
                    // Thiếu [C] ở đây!
                    AddPara(body, "[D] whole");
                    AddPara(body, "[ANS:B]");

                    AddQuestion(body, 106, "Mr. Tanaka asked his assistant to arrange a meeting between both firms.", "among", "between", "through", "beside", "B");
                    AddQuestion(body, 107, "The warranty covers parts and labor for any defect that occurs under normal conditions.", "normal", "normally", "normalize", "normality", "A");
                    
                    // LỖI 2: CÂU 108 NHẢY VỌT LÊN CÂU 110 (BỎ QUA CÂU 109)!
                    AddQuestion(body, 108, "Because of severe winter weather, tomorrow's flight has been canceled.", "temporarily", "temporary", "temperance", "temperament", "A");
                    
                    // Nhảy sang câu 110:
                    AddQuestion(body, 110, "The human resources division has announced an opening for an experienced designer.", "extremely", "extreme", "extremity", "extremes", "A");

                    // LỖI 3: Part 6 có câu hỏi nằm ngoài nhóm [GROUP_START]...[GROUP_END]
                    AddPara(body, "[PART:6]");
                    AddPara(body, "[DIRECTIONS] Read the texts that follow.");
                    AddQuestion(body, 111, "This question in Part 6 is wrongly located outside a group.", "incorrectly", "correctly", "mistakenly", "properly", "A");

                    mainPart.Document.Save();
                }

                return mem.ToArray();
            }
        }

        private static void AddQuestion(Body body, int num, string qText, string a, string b, string c, string d, string ans)
        {
            AddPara(body, string.IsNullOrEmpty(qText) ? $"[Q:{num}]" : $"[Q:{num}] {qText}");
            AddPara(body, $"[A] {a}");
            AddPara(body, $"[B] {b}");
            AddPara(body, $"[C] {c}");
            AddPara(body, $"[D] {d}");
            AddPara(body, $"[ANS:{ans}]");
        }

        private static void AddPara(Body body, string text)
        {
            var p = body.AppendChild(new Paragraph());
            var r = p.AppendChild(new Run());
            r.AppendChild(new Text(text));
        }
    }
}
