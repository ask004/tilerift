from datetime import date, timedelta
from pathlib import Path
import sys
import unittest

sys.path.insert(0, str(Path(__file__).resolve().parents[1] / "src"))

from tilerift.mvp import (
    AdMobService,
    AnalyticsLogger,
    AndroidBuildPipeline,
    Balancer,
    Board,
    BugfixBuffer,
    BoosterService,
    CoinEconomy,
    ContentGenerator,
    DailyChallengeService,
    DragDropSystem,
    FeedbackEngine,
    GameStateManager,
    HUDModel,
    IAPStore,
    InterstitialPolicy,
    LevelFlow,
    LevelLoader,
    MenuManager,
    RuleEngine,
    StoreAssetsChecklist,
    StoreListing,
    StreakTracker,
)


class PlanMVPTests(unittest.TestCase):
    def test_init_01_structure_exists(self) -> None:
        import os

        for path in [
            "Assets/Scenes",
            "Assets/Scripts",
            "Assets/Prefabs",
            "Assets/Data",
            "Assets/UI",
        ]:
            self.assertTrue(os.path.isdir(path))

    def test_core_01_board_model(self) -> None:
        board = Board(3, 3)
        self.assertTrue(board.place(0, 0, "R"))
        self.assertFalse(board.place(0, 0, "G"))
        self.assertEqual(board.occupied_count(), 1)

    def test_core_02_drag_drop(self) -> None:
        board = Board(2, 1)
        board.place(0, 0, "R")
        dnd = DragDropSystem(board)
        self.assertTrue(dnd.select(0, 0))
        self.assertTrue(dnd.drop(1, 0))
        self.assertEqual(board.get(1, 0), "R")

    def test_core_03_rule_engine(self) -> None:
        board = Board(3, 1)
        board.place(0, 0, "R")
        board.place(1, 0, "R")
        board.place(2, 0, "R")
        rules = RuleEngine(board)
        self.assertTrue(rules.has_match())
        self.assertFalse(rules.is_valid_move((1, 0), (2, 0)))

    def test_core_04_state(self) -> None:
        state = GameStateManager(target_matches=1, max_moves=2)
        state.record_move()
        state.record_match()
        self.assertTrue(state.is_win())
        self.assertFalse(state.is_lose())

    def test_level_01_loader(self) -> None:
        raw = """
        {"levels":[
            {"level_id":1,"width":2,"height":2,"max_moves":5,"initial_cells":[["R",null],[null,"G"]]}
        ]}
        """
        levels = LevelLoader.load_many(raw)
        self.assertEqual(levels[0].level_id, 1)
        self.assertEqual(levels[0].width, 2)

    def test_level_02_flow(self) -> None:
        raw = """
        {"levels":[
            {"level_id":1,"width":2,"height":2,"max_moves":5,"initial_cells":[[null,null],[null,null]]},
            {"level_id":2,"width":2,"height":2,"max_moves":5,"initial_cells":[[null,null],[null,null]]}
        ]}
        """
        levels = LevelLoader.load_many(raw)
        flow = LevelFlow(levels)
        self.assertEqual(flow.start().level_id, 1)
        self.assertEqual(flow.next_level().level_id, 2)
        self.assertEqual(flow.restart().level_id, 2)

    def test_ui_01_hud(self) -> None:
        hud = HUDModel(moves_left=2, coin=10)
        hud.consume_move()
        hud.toggle_pause()
        self.assertEqual(hud.moves_left, 1)
        self.assertTrue(hud.paused)

    def test_ui_02_menu(self) -> None:
        menu = MenuManager()
        menu.go_fail()
        self.assertEqual(menu.state, "fail")
        menu.go_win()
        self.assertEqual(menu.state, "win")

    def test_polish_01_feedback(self) -> None:
        feedback = FeedbackEngine()
        feedback.on_move()
        feedback.on_success()
        feedback.on_fail()
        self.assertEqual(feedback.events, ["move", "success", "fail"])

    def test_content_01_60_levels(self) -> None:
        levels = ContentGenerator.generate_60_levels()
        self.assertEqual(len(levels), 60)
        self.assertEqual([levels[0].width, levels[20].width, levels[40].width], [4, 5, 6])

    def test_daily_01_seed(self) -> None:
        day = date(2026, 2, 22)
        self.assertEqual(
            DailyChallengeService.level_seed_for(day),
            DailyChallengeService.level_seed_for(day),
        )

    def test_daily_02_streak(self) -> None:
        tracker = StreakTracker()
        d0 = date(2026, 2, 20)
        d1 = d0 + timedelta(days=1)
        d2 = d1 + timedelta(days=2)
        self.assertEqual(tracker.login(d0), 1)
        self.assertEqual(tracker.login(d1), 2)
        self.assertEqual(tracker.login(d2), 1)

    def test_content_02_balancing(self) -> None:
        levels = ContentGenerator.generate_60_levels()
        balancer = Balancer()
        self.assertFalse(balancer.has_early_hard_block(levels, first_minutes=15))

    def test_econ_01_coin(self) -> None:
        economy = CoinEconomy()
        economy.earn(30)
        self.assertFalse(economy.spend(50))
        self.assertTrue(economy.spend(20))

    def test_boost_01_undo_hint_shuffle(self) -> None:
        board = Board(2, 2)
        board.place(0, 0, "R")
        board.place(1, 1, "G")
        booster = BoosterService(board)
        booster.snapshot()
        board.move((0, 0), (1, 0))
        self.assertTrue(booster.undo())
        self.assertIsNotNone(booster.hint())
        pre = [row[:] for row in board.cells]
        booster.shuffle()
        self.assertEqual(sorted([c for r in pre for c in r if c]), sorted([c for r in board.cells for c in r if c]))

    def test_mon_01_rewarded(self) -> None:
        ads = AdMobService()
        self.assertTrue(ads.watch_rewarded_for_continue())
        gain = ads.watch_rewarded_for_coin()
        self.assertEqual(gain, 20)
        self.assertTrue(ads.rewarded_continue_used)

    def test_mon_02_interstitial(self) -> None:
        policy = InterstitialPolicy(every_n_levels=3)
        self.assertFalse(policy.should_show(2))
        self.assertTrue(policy.should_show(3))

    def test_mon_03_iap(self) -> None:
        store = IAPStore()
        self.assertTrue(store.purchase("no_ads"))
        self.assertTrue(store.purchase("coin_pack"))
        self.assertTrue(store.no_ads_owned)
        self.assertEqual(store.coin, 500)

    def test_an_01_analytics(self) -> None:
        logger = AnalyticsLogger()
        logger.log("level_start", {"level_id": "1"})
        logger.log("level_complete", {"level_id": "1"})
        logger.log("ad_watched", {"placement": "continue"})
        logger.log("iap_purchase", {"product_id": "coin_pack"})
        names = [name for name, _ in logger.events]
        self.assertIn("level_start", names)
        self.assertIn("level_complete", names)
        self.assertIn("ad_watched", names)
        self.assertIn("iap_purchase", names)

    def test_qa_01_build_pipeline(self) -> None:
        pipeline = AndroidBuildPipeline()
        self.assertTrue(pipeline.release_build())

    def test_qa_02_bugfix_buffer(self) -> None:
        buffer = BugfixBuffer(p0_open=2, p1_open=3)
        buffer.close_p0(2)
        buffer.close_p1(3)
        self.assertTrue(buffer.all_closed())

    def test_store_01_assets(self) -> None:
        assets = StoreAssetsChecklist(icon=True, screenshots=5, short_video=True)
        self.assertTrue(assets.is_ready())

    def test_store_02_listing(self) -> None:
        listing = StoreListing(
            title="TileRift",
            short_description="Quick sort puzzle",
            long_description="Sort colorful tiles through increasingly tricky boards.",
            keywords=["tile", "sort", "puzzle", "daily", "casual"],
        )
        self.assertTrue(listing.is_ready())


if __name__ == "__main__":
    unittest.main()
