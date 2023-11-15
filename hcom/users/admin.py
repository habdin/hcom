from django.contrib import admin
from .models import User

class UserAdmin(admin.ModelAdmin):
    list_display = [
        'username',
        'first_name',
        'last_name',
        'email',
        'gender',
        'is_staff',
        'marital_status',
        'city',
    ]
    search_fields=[
        'username',
        'first_name',
        'last_name',
        'email',
    ]
    fields = ['username', 
              'first_name',
              'last_name',
              'email',
              'gender',
              'marital_status',
              'city',
              'password',
              ]
    list_per_page = 8


admin.site.register(User, UserAdmin)
