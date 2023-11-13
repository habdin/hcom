from django.conf import settings
from django.conf.urls.static import static
from django.contrib import admin
from django.contrib.auth import views as auth_views
from django.urls import include, path
from rest_framework import routers
from users.forms import UserLoginForm
from users.views import UserViewSet

router = routers.DefaultRouter()
router.register(r'users', UserViewSet)

urlpatterns = [
    path('', include('base.urls')),
    path('auth/login/',
         auth_views.LoginView.as_view(template_name="users/user_login.html"),
         name='user-login'),
    path('clinic/', include('clinic.urls')),
    path('users/', include('users.urls')),
    path('pharmacy/', include('pharmacy.urls')),
    path('admin/', admin.site.urls),
    path('rest/', include(router.urls)),
    path('dummy/', include('dummy.urls')),
] + static(settings.MEDIA_URL, document_root=settings.MEDIA_ROOT
           )  # To serve image correct paths in DEBUG MODE
